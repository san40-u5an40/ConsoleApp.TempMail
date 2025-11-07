namespace TempMailClient;

public partial class MailClient
{
    private ApiMethods apiMethods;

    public MailClient(HttpClientHandler? httpClientHandler = null, bool disposeHandler = true) =>
        apiMethods = new ApiMethods(httpClientHandler, disposeHandler);

    private string? token;
    private AccountInfo? accountInfo;
    private MessageList? messageList;

    public string? Token => token;
    public AccountInfo? AccountInfo => accountInfo;
    public MessageList? MessageList => messageList;
}