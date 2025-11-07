namespace TempMailApp.Core;

internal partial class App
{
    private async Task MessagesMenuAsync()
    {
        Dictionary<int, string> menuItems = new()
        {
            { 1, "Выйти из приложения" },
            { 2, "Удалить аккаунт" },
            { 3, "Обновить список" }
        };
        ControllerMessagesMenu messagesController = new(menuItems, 1, mailClient, $"Меню сообщений {mailClient.AccountInfo!.Address}:");
        Printer<(int Value, bool IsMessage, MessageContent? MessageContent, bool IsDelete)> printer = new(upperDisplay, messagesController);

        (int Value, bool IsMessage, MessageContent? MessageContent, bool IsDelete) resultPrinter;

        do
        {
            await MessagesUpdateAsync(messagesController);
            resultPrinter = await printer.ShowAsync();

            switch (resultPrinter.Value, resultPrinter.IsMessage, resultPrinter.MessageContent, resultPrinter.IsDelete)
            {
                case (_, true, MessageContent messageContent, false):
                    await MessageOpenedAsync(messageContent);
                    break;

                case (_, true, MessageContent messageContent, true):
                    var resultMessageDeleted = await mailClient.DeleteMessageAsync(messageContent.Id!);
                    if (!resultMessageDeleted.IsSuccess)
                        await InfoAsync(resultMessageDeleted.ErrorMessage!);
                    else
                        messagesController.UpdateMessageData();
                    break;

                case (1, false, _, _):
                    return;

                case (2, false, _, _):
                    var resultAccountDeleted = await mailClient.DeleteAccountAsync();
                    if (!resultAccountDeleted.IsSuccess)
                    {
                        await InfoAsync(resultAccountDeleted.ErrorMessage!);
                    }
                    else
                    {
                        await InfoAsync("Аккаунт удалён. Необходимо заново запустить приложение.");
                        return;
                    }
                    break;

                case (3, false, _, _):
                    await MessagesUpdateAsync(messagesController);
                    break;
            }
        }
        while (true);
    }

    private async Task MessagesUpdateAsync(ControllerMessagesMenu messagesController)
    {
        var resultMessagesUpdated = await mailClient.UpdateMessagesAsync();
        if (!resultMessagesUpdated.IsSuccess)
            await InfoAsync(resultMessagesUpdated.ErrorMessage!);
        else
            messagesController.UpdateMessageData();
    }
}