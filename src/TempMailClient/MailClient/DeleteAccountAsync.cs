namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteAccountAsync()
    {
        if (accountInfo == null || token == null)
            return (false, "Для удаления аккаунта необходимо произвести вход!");

        var result = await apiMethods.DeleteAccountAsync(token, accountInfo.Id!);
        if(!result.IsSuccess)
            return (false, result.ErrorMessage);

        token = null;
        accountInfo = null;
        messageList = null;

        return (true, null);
    }
}