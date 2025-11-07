namespace TempMailClient;

public partial class MailClient
{
    private async Task<(bool IsSuccess, string? ErrorMessage)> UpdateAccountInfoAsync()
    {
        if (token == null)
            return (false, "Для обновления информации об аккаунте необходимо произвести вход!");

        var result = await apiMethods.GetMeAsync(token);
        if(!result.IsSuccess)
            return (false, result.ErrorMessage);

        accountInfo = result.AccountInfo;
        return (true, null);
    }
}