internal partial class ApiMethods
{
    private const string HOST = "api.mail.gw";

    private HttpClient httpClient;

    internal ApiMethods(HttpClientHandler? httpClientHandler = null, bool disposeHandler = true)
    {
        if(httpClientHandler != null)
            httpClient = new HttpClient(httpClientHandler, disposeHandler);
        else
            httpClient = new HttpClient();
    }    
}