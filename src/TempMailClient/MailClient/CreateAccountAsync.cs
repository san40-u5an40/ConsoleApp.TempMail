namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, string? ErrorMessage)> CreateAccountAsync(string address, string password) =>
        await apiMethods.PostAccountAsync(address, password);
}