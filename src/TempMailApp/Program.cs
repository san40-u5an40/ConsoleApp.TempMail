using TempMailApp.Core;

internal class Program
{
    static async Task Main(string[] args)
    {
        App tempMailApp = new(new MailClient(null), args);
        await tempMailApp.StartAsync();
    }
}