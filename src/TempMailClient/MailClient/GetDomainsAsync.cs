namespace TempMailClient;

public partial class MailClient
{
    public async Task<(bool IsSuccess, DomainList? DomainList, string? ErrorMessage)> GetDomainsAsync() =>
        await apiMethods.GetDomainsAsync();
}