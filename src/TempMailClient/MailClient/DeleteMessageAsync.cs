namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteMessageAsync(string id)
    {
        if (token == null)
            return (false, "Для удаления сообщений необходимо произвести вход!");

        var resultDeleteMessage = await apiMethods.DeleteMessageAsync(token, id);
        if (!resultDeleteMessage.IsSuccess)
            return (false, "Ошибка удаления сообщения: " + resultDeleteMessage.ErrorMessage);

        var resultUpdateMessage = await UpdateMessagesAsync();
        if (!resultUpdateMessage.IsSuccess)
            return (false, "Ошибка обновления сообщений: " + resultUpdateMessage.ErrorMessage);

        return (true, null);
    }
}