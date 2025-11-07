namespace TempMailClient.ApiData;

public class Token
{
    [JsonPropertyName("token")]
    public string? Value { get; set; }
}