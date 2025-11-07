using san40_u5an40.ConsoleDisplayFramework.Interfaces;

namespace TempMailApp.FrameworkControllersExtension;

internal class ControllerMessagesMenu : IControllable<(int Value, bool IsMessage, MessageContent? MessageContent, bool IsDelete)>
{
    private const int MINIMAL_CONSOLE_SIZE = 80;

    private string? menuMessage;
    private bool isExit = false;

    private Dictionary<int, string> menuItems = new();
    private int menuMaxKeyItem;

    private MailClient mailClient;
    private Dictionary<int, (string Subject, string From, string Date)> messageItems = new();

    private (int Value, bool IsMessage, MessageContent? MessageContent, bool IsDelete) currentMenuItem;

    internal ControllerMessagesMenu(Dictionary<int, string> menuItems, int startItemValue, MailClient mailClient, string? menuMessage = null)
    {
        var result = IsValid(menuItems, startItemValue);
        if (!result.IsValid)
            throw new ArgumentException(result.ErrorMessage);

        this.menuItems = menuItems;
        this.mailClient = mailClient;
        this.menuMessage = menuMessage;

        currentMenuItem = (startItemValue, false, null, false);
        menuMaxKeyItem = menuItems.MaxBy(p => p.Key).Key;

        static (bool IsValid, string? ErrorMessage) IsValid(Dictionary<int, string> menuItems, int startItemValue)
        {
            if (menuItems.Count == 0)
                return (false, "Для составления меню необходимо передать непустую коллекцию!");

            if (!menuItems.ContainsKey(startItemValue))
                return (false, "Необходимо указать стартовое значение меню, которое в нём содержится!");

            menuItems = menuItems
                .OrderBy(p => p.Key)
                .ToDictionary();

            int minKey = menuItems.MinBy(p => p.Key).Key;
            int maxKey = menuItems.MaxBy(p => p.Key).Key;

            if (maxKey - minKey + 1 != menuItems.Count)
                return (false, "Необходимо указать коллекцию с непрерывно возрастающей последовательностью чисел!");

            return (true, null);
        }
    }

    public bool IsExit => isExit;
    public (int Value, bool IsMessage, MessageContent? MessageContent, bool IsDelete) ControlValue => currentMenuItem;

    public void Print()
    {
        if (Console.WindowWidth < MINIMAL_CONSOLE_SIZE)
            return;

        if (menuMessage != null)
            Console.WriteLine(menuMessage + '\n');

        foreach (var item in menuItems)
        {
            if (item.Key == currentMenuItem.Value)
                ConsoleExtension.WriteColor(item.Key + ") " + item.Value + " ←\n", ConsoleColor.DarkRed);
            else
                Console.WriteLine(item.Key + ") " + item.Value);
        }

        Console.Write('\n');

        Console.WriteLine("Количество сообщений: " + messageItems.Count);
        foreach (var item in messageItems)
        {
            switch ((item.Key == currentMenuItem.Value, currentMenuItem.IsDelete))
            {
                case (true, false):
                    ConsoleExtension.WriteColor(GetItemString(item.Value), ConsoleColor.DarkRed);
                    Console.Write(" | ");
                    Console.Write("del");
                    Console.Write('\n');
                    break;

                case (true, true):
                    Console.Write(GetItemString(item.Value));
                    Console.Write(" | ");
                    ConsoleExtension.WriteColor("del", ConsoleColor.DarkRed);
                    Console.Write('\n');
                    break;

                case (false, _):
                    Console.WriteLine(GetItemString(item.Value) + " | " + "del");
                    break;
            }
        }

        static string GetItemString((string Subject, string From, string Date) item)
        {
            bool tryParseResult = DateTime.TryParse(item.Date, out DateTime messageDate);

            return
                $"{item.Subject.Reduce(30).Message,-30} | " +
                $"{item.From.Reduce(30).Message,-30} | " +
                $"{(tryParseResult ? messageDate.ToString("g") : item.Date),-20}";
        }
    }

    public void UpdateMessageData()
    {
        if (mailClient.MessageList != null)
        {
            messageItems.Clear();

            var cnt = Counter.GetCounter(menuMaxKeyItem + 1);

            foreach (var messageItem in mailClient.MessageList.Messages)
            {
                if (messageItem.IsDeleted)
                    continue;

                messageItems.Add(cnt(), (messageItem.Subject!, messageItem.From!.Name!, messageItem.CreatedAt!));
            } 
        }
    }

    public void StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                ChangeItem(key);

            if (key == ConsoleKey.Enter)
            {
                ChangeCurrentItem();
                isExit = true;
            }
        }
    }

    private void ChangeItem(ConsoleKey key)
    {
        if (key == ConsoleKey.UpArrow && (menuItems.ContainsKey(currentMenuItem.Value - 1) || messageItems.ContainsKey(currentMenuItem.Value - 1)))
            currentMenuItem.Value--;

        else if (key == ConsoleKey.DownArrow && (menuItems.ContainsKey(currentMenuItem.Value + 1) || messageItems.ContainsKey(currentMenuItem.Value + 1)))
            currentMenuItem.Value++;

        else if (key == ConsoleKey.LeftArrow && IsCurrentMenuItemMessage() && currentMenuItem.IsDelete == true)
            currentMenuItem.IsDelete = false;

        else if (key == ConsoleKey.RightArrow && IsCurrentMenuItemMessage() && currentMenuItem.IsDelete == false)
            currentMenuItem.IsDelete = true;
    }

    private void ChangeCurrentItem()
    {
        int currValue = currentMenuItem.Value;
        bool currIsMessage = IsCurrentMenuItemMessage();
        MessageContent? currMessage = null;
        bool currIsDelete = false;

        if (currIsMessage)
        {
            int messageIndex = currValue - menuMaxKeyItem - 1;
            currMessage = mailClient.MessageList!.Messages[messageIndex];

            currValue = menuMaxKeyItem;

            currIsDelete = currentMenuItem.IsDelete;
        }
        
        currentMenuItem = (currValue, currIsMessage, currMessage, currIsDelete);
    }

    private bool IsCurrentMenuItemMessage() =>
        messageItems.ContainsKey(currentMenuItem.Value);
}