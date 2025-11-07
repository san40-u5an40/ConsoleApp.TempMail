namespace TempMailApp.Core;

internal partial class App
{
    private async Task LoginMenuAsync()
    {
        ControllerReadLine menu;
        Printer<string> printer;

        menu = new ControllerReadLine("Введите email (данные не будут отображаться): ");
        printer = new(upperDisplay, menu, false);
        string email = await printer.ShowAsync();

        menu = new ControllerReadLine("Введите пароль (данные не будут отображаться): ");
        printer = new(upperDisplay, menu, false);
        string password = await printer.ShowAsync();

        var result = await mailClient.LoginAsync(email, password);

        if (!result.IsSuccess)
        {
            await InfoAsync(result.ErrorMessage!);
            return;
        }

        await MessagesMenuAsync();
    }
}