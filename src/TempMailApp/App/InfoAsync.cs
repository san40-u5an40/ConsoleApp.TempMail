namespace TempMailApp.Core;

internal partial class App
{
    private async Task InfoAsync(string input)
    {
        var infoMessage = input + "\nДля продолжения нажмите [enter]...";
        ControllerPressEnter enterController = new(infoMessage);
        Printer<bool> printer = new(upperDisplay, enterController, false);
        await printer.ShowAsync();
    }
}