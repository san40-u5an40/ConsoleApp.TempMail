using san40_u5an40.ConsoleDisplayFramework.Interfaces;

namespace TempMailApp.FrameworkControllersExtension;

internal class ControllerMessageOpened : IControllable<bool>
{
    private const int MINIMAL_CONSOLE_SIZE = 40;
    private bool isExit = false;
    private MessageContent messageContent;

    internal ControllerMessageOpened(MessageContent messageContent) =>
        this.messageContent = messageContent;

    public bool IsExit => isExit;
    public bool ControlValue => true;

    public void Print()
    {
        if (Console.WindowWidth < MINIMAL_CONSOLE_SIZE)
            return;

        string toStr = string.Join("\n      ", messageContent.To!.Select(p => p.Address));
        bool tryParseResult = DateTime.TryParse(messageContent.CreatedAt, out DateTime messageDate);

        var str = new StringBuilder()
            .AppendLine("От кого: " + messageContent.From!.Name + " (" + messageContent.From!.Address + ')')
            .AppendLine("Кому: " + toStr)
            .AppendLine()
            .AppendLine(messageContent.Intro)
            .AppendLine()
            .AppendLine("Размер: " + messageContent.Size)
            .AppendLine("Дата создания: " + (tryParseResult ? messageDate.ToString("g") : messageContent.CreatedAt))
            .AppendLine()
            .AppendLine("Для выхода нажмите [escape] или [enter]...");

        Console.Write(str.ToString());
    }

    public void StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Enter || key == ConsoleKey.Escape)
                isExit = true;
        }
    }
}