internal partial class ApiMethods
{
    private const string TOKEN = "/token";

    internal async Task<(bool IsSuccess, string? Token, string? ErrorMessage)> PostTokenAsync(string address, string password)
    {
        AccountRequestInfo accountData = new()
        {
            Address = address,
            Password = password
        };

        UriBuilder uriToken = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = TOKEN
        };

        HttpRequestMessage request = new()
        {
            RequestUri = uriToken.Uri,
            Method = HttpMethod.Post,
            Content = JsonContent.Create(accountData)
        };

        await request.Content.LoadIntoBufferAsync();

        bool isSuccess = false;
        Token? token = null;
        string? errorMessage = null;

        try
        {
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            isSuccess = response.IsSuccessStatusCode;

            if (isSuccess)
                token = await response.Content.ReadFromJsonAsync<Token>();
            else
                errorMessage = "Ответ со стороны сервера: " + response.ReasonPhrase;
        }
        catch (Exception ex)
        {
            return (false, null, "Внутренняя ошибка: " + ex.Message);
        }

        return (isSuccess, token?.Value, errorMessage);
    }
}