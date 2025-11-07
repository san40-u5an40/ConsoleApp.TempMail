namespace TempMailClient.ApiData;

public class DomainList
{
    [JsonPropertyName("hydra:member")]
    public List<DomainInfo> Domains { get; set; } = new();

    [JsonPropertyName("hydra:totalItems")]
    public int Length { get; set; }
}

public class DomainInfo
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("domain")]
    public string? Name { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}