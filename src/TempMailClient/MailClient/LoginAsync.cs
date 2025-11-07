namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, string? ErrorMessage)> LoginAsync(string address, string password)
    {
        var resultPostToken = await apiMethods.PostTokenAsync(address, password);
        if (!resultPostToken.IsSuccess)
            return (false, "Ошибка получения токена: " + resultPostToken.ErrorMessage);
        token = resultPostToken.Token;

        var resultUpdateAccount = await UpdateAccountInfoAsync();
        if (!resultUpdateAccount.IsSuccess)
            return (false, "Ошибка обновления данных пользователя: " + resultUpdateAccount.ErrorMessage);

        return (true, null);
    }
}