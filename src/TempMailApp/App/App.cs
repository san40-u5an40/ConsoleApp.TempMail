namespace TempMailApp.Core;

internal partial class App
{
    private string[] args;
    private MailClient mailClient;

    private UpperDisplay upperDisplay = new UpperDisplay()
            .AppendEmpty('-')
            .Append('-', "Добро пожаловать в программу")
            .Append('-', "TempMailClient")
            .AppendEmpty('-');

    internal App(MailClient mailClient, string[] args) =>
        (this.mailClient, this.args) = (mailClient, args);
}
