namespace TempMailApp.Core;

internal partial class App
{
    private async Task CreateAccountMenuAsync()
    {
        var resultGetDomains = await mailClient.GetDomainsAsync();
        if (!resultGetDomains.IsSuccess)
        {
            await InfoAsync(resultGetDomains.ErrorMessage!);
            return;
        }

        Dictionary<int, string> domains = GetDomainsDictionary(resultGetDomains.DomainList!);
        if (domains.Count == 0)
        {
            await InfoAsync("Непредвиденная ошибка создания словаря доменов!");
            return;
        }

        ControllerMenu domainsMenu = new(domains, 1, "Выберите домен временного почтового ящика:");
        Printer<int> domainsPrinter = new(upperDisplay, domainsMenu);
        int domainKey = await domainsPrinter.ShowAsync();

        ControllerReadLine userNameInputController = new("Введите имя пользователя: ", true);
        Printer<string> userNameInputPrinter = new(upperDisplay, userNameInputController);
        string userName = await userNameInputPrinter.ShowAsync();

        ControllerReadLine passwordInputController = new($"Введите пароль (данные не будут отображаться): ");
        Printer<string> userPasswordPrinter = new(upperDisplay, passwordInputController, false);
        string userPassword = await userPasswordPrinter.ShowAsync();

        string userEmail = userName + '@' + domains[domainKey];
        var resultCreatedAccount = await mailClient.CreateAccountAsync(userEmail, userPassword);
        if (!resultCreatedAccount.IsSuccess)
        {
            await InfoAsync(resultCreatedAccount.ErrorMessage!);
            return;
        }

        var resultLoginAccount = await mailClient.LoginAsync(userEmail, userPassword);
        if (!resultCreatedAccount.IsSuccess)
        {
            await InfoAsync(resultLoginAccount.ErrorMessage!);
            return;
        }

        await InfoAsync("Сохраните данные для входа в учётную запись \"" + userEmail + "\"");
        await MessagesMenuAsync();

        // Локальная функция получения словаря доменов
        static Dictionary<int, string> GetDomainsDictionary(DomainList domainList)
        {
            Dictionary<int, string> domains = new();
            var cnt = Counter.GetCounter(1);

            foreach (DomainInfo domain in domainList.Domains)
            {
                if (!domain.IsActive)
                    continue;

                domains.Add(cnt(), domain.Name!);
            }

            return domains;
        }
    }
}