internal partial class ApiMethods
{
    private const string MESSAGES = "/messages";
    private MessageSeen messageSeen = new();

    internal async Task<(bool IsSuccess, MessageList? MessagesList, string? ErrorMessage)> GetMessagesAsync(string token)
    {
        UriBuilder uriGetMessages = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = MESSAGES
        };

        HttpRequestMessage request = new();
        request.Method = HttpMethod.Get;
        request.Headers.Add("Authorization", $"Bearer {token}");

        MessageList messagesList = new();

        try
        {
            int pageRequest = 0;

            do
            {
                uriGetMessages.Query = $"?page={++pageRequest}";
                request.RequestUri = uriGetMessages.Uri;

                using var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode || response.Content == null)
                    return (false, null, "Ответ со стороны сервера: " + response.ReasonPhrase);

                var tempMessagesList = await response.Content.ReadFromJsonAsync<MessageList>();

                foreach (MessageContent tempItem in tempMessagesList!.Messages)
                    messagesList.Messages.Add(tempItem);
                messagesList.Length = tempMessagesList.Length;
            }
            while (messagesList.Messages.Count < messagesList.Length);
        }
        catch (Exception ex)
        {
            return (false, null, "Внутренняя ошибка: " + ex.Message);
        }

        return (true, messagesList, null);
    }

    internal async Task<(bool IsSuccess, string? ErrorMessage)> DeleteMessageAsync(string token, string id)
    {
        HttpRequestMessage request = GetMessagesRequest(HttpMethod.Delete, token, id);

        bool isSuccess = false;
        string? errorMessage = null;

        try
        {
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            isSuccess = response.IsSuccessStatusCode;

            if(!isSuccess)
                errorMessage = "Ответ со стороны сервера: " + response.ReasonPhrase;
        }
        catch (Exception ex)
        {
            return (false, "Внутренняя ошибка: " + ex.Message);
        }

        return (isSuccess, errorMessage);
    }

    private HttpRequestMessage GetMessagesRequest(HttpMethod httpMethod, string token, string id)
    {
        UriBuilder uriWithId = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = MESSAGES,
            Query = $"?id={id}"
        };

        HttpRequestMessage request = new()
        {
            Method = httpMethod,
            RequestUri = uriWithId.Uri
        };

        request.Headers.Add("Authorization", $"Bearer {token}");

        return request;
    }
}