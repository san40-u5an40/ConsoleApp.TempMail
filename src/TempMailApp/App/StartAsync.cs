namespace TempMailApp.Core;

internal partial class App
{
    private const string HELP_INFO = @"Для использования программы необходимо:
 - Запустить её без аргументов для классического сценария использования (с выбором: Вход / Создание аккаунта).
 - Передать при запуске в аргументах логин и пароль для входа в существующий аккаунт.";

    internal async Task StartAsync()
    {
        Console.Title = "TempMail";

        if (args.Length == 0)
        {
            await StartMenuAsync();
            return;
        }

        if (args.Length == 2 && !args.Any(string.IsNullOrEmpty))
        {
            var result = await mailClient.LoginAsync(args[0], args[1]);
            if (!result.IsSuccess)
            {
                await InfoAsync(result.ErrorMessage!);
                return;
            }

            await MessagesMenuAsync();
            return;
        }

        await InfoAsync(HELP_INFO);
    }
}