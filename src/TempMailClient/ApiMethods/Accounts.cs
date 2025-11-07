internal partial class ApiMethods
{
    private const string ACCOUNTS = "/accounts";
    private const string ME = "/me";

    internal async Task<(bool IsSuccess, string? ErrorMessage)> PostAccountAsync(string address, string password)
    {
        AccountRequestInfo accountRequestInfo = new()
        {
            Address = address,
            Password = password
        };

        UriBuilder uriPostAccounts = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = ACCOUNTS
        };

        HttpRequestMessage request = new()
        {
            RequestUri = uriPostAccounts.Uri,
            Method = HttpMethod.Post,
            Content = JsonContent.Create(accountRequestInfo)
        };

        await request.Content.LoadIntoBufferAsync();

        bool isSuccess = false;
        string? errorMessage = null;

        try
        {
            using HttpResponseMessage response = await httpClient.SendAsync(request);

            isSuccess = response.IsSuccessStatusCode;

            if (!isSuccess)
                errorMessage = "Ответ со стороны сервера: " + response.ReasonPhrase;
        }
        catch(Exception ex)
        {
            return (false, "Внутренняя ошибка: " + ex.Message);
        }

        return (isSuccess, errorMessage);
    }

    internal async Task<(bool IsSuccess, AccountInfo? AccountInfo, string? ErrorMessage)> GetMeAsync(string token)
    {
        UriBuilder uriGetMe = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = ME
        };

        HttpRequestMessage request = new()
        {
            RequestUri = uriGetMe.Uri,
            Method = HttpMethod.Get
        };
        request.Headers.Add("Authorization", $"Bearer {token}");

        bool isSuccess = false;
        AccountInfo? accountResponseInfo = null;
        string? errorMessage = null;

        try
        {
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            isSuccess = response.IsSuccessStatusCode;

            if (isSuccess)
                accountResponseInfo = await response.Content.ReadFromJsonAsync<AccountInfo>();
            else
                errorMessage = "Ответ со стороны сервера: " + response.ReasonPhrase;
        }
        catch (Exception ex)
        {
            return (false, null, "Внутренняя ошибка: " + ex.Message);
        }

        return (isSuccess, accountResponseInfo, errorMessage);
    }

    internal async Task<(bool IsSuccess, string? ErrorMessage)> DeleteAccountAsync(string token, string id)
    {
        UriBuilder uriDeleteAccounts = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = ACCOUNTS,
            Query = $"?id={id}"
        };

        HttpRequestMessage request = new()
        {
            RequestUri = uriDeleteAccounts.Uri,
            Method = HttpMethod.Delete
        };
        request.Headers.Add("Authorization", $"Bearer {token}");

        bool isSuccess = false;
        string? errorMessage = null;

        try
        {
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            isSuccess = response.IsSuccessStatusCode;

            if (!isSuccess)
                errorMessage = "Ответ со стороны сервера: " + response.ReasonPhrase;
        }
        catch (Exception ex)
        {
            return (false, "Внутренняя ошибка: " + ex.Message);
        }

        return (isSuccess, errorMessage);
    }
}