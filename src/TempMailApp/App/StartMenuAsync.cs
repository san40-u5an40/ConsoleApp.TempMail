namespace TempMailApp.Core;

internal partial class App
{
    private async Task StartMenuAsync()
    {
        ControllerMenu menu = new(new Dictionary<int, string>
        {
            { 1, "Войти в существующий аккаунт" },
            { 2, "Создать аккаунт" },
            { 3, "Выйти из приложения" }
        }, 1, "Меню:");

        Printer<int> printer = new(upperDisplay, menu);
        int result = await printer.ShowAsync();

        if (result == 3)
            return;

        if (result == 1)
        {
            await LoginMenuAsync();
            return;
        }

        if (result == 2)
        {
            await CreateAccountMenuAsync();
            return;
        }
    }
}