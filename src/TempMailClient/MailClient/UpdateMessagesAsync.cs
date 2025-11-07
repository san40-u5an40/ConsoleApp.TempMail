namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessagesAsync()
    {
        if (token == null)
            return (false, "Для получения сообщений необходимо произвести вход!");

        var result = await apiMethods.GetMessagesAsync(token);
        if(!result.IsSuccess)
            return (false, result.ErrorMessage);

        messageList = result.MessagesList;
        return (true, null);
    }
}