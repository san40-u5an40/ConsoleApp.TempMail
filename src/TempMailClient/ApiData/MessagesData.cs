namespace TempMailClient.ApiData;

public class MessageList
{
    [JsonPropertyName("hydra:member")]
    public List<MessageContent> Messages { get; set; } = new();

    [JsonPropertyName("hydra:totalItems")]
    public int Length { get; set; }
}

public class MessageContent
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("intro")]
    public string? Intro { get; set; }

    [JsonPropertyName("from")]
    public MessageContributor? From { get; set; }

    [JsonPropertyName("to")]
    public List<MessageContributor>? To { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("downloadUrl")]
    public string? DownloadPath { get; set; }
}

public class MessageContributor
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }
}

public class MessageSeen
{
    [JsonPropertyName("seen")]
    public bool Seen { get; set; } = true;
}