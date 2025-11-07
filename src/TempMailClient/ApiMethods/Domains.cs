internal partial class ApiMethods
{
    private const string DOMAINS = "/domains";

    internal async Task<(bool IsSuccess, DomainList? DomainList, string? ErrorMessage)> GetDomainsAsync()
    {
        UriBuilder uriDomains = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = HOST,
            Path = DOMAINS
        };

        DomainList domainsList = new();

        try
        {
            int pageRequest = 0;

            do
            {
                uriDomains.Query = $"?page={++pageRequest}";

                using var response = await httpClient.GetAsync(uriDomains.Uri);
                if (!response.IsSuccessStatusCode || response.Content == null)
                    return (false, null, "Ответ со стороны сервера: " + response.ReasonPhrase);

                var tempDomainList = await response.Content.ReadFromJsonAsync<DomainList>();

                foreach (var tempItem in tempDomainList!.Domains)
                    domainsList.Domains.Add(tempItem);
                domainsList.Length = tempDomainList.Length;
            }
            while (domainsList.Domains.Count < domainsList.Length);
        }
        catch (Exception ex)
        {
            return (false, null, "Внутренняя ошибка: " + ex.Message);
        }

        return (true, domainsList, null);
    }
}