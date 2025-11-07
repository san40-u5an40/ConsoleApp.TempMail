namespace TempMailApp.Core;

internal partial class App
{
    private async Task MessageOpenedAsync(MessageContent messageContent)
    {
        ControllerMessageOpened messageController = new(messageContent);
        Printer<bool> printer = new(upperDisplay, messageController);
        await printer.ShowAsync();
    }
}