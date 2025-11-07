namespace TempMailClient;

public partial class MailClient
{
    public bool Logout()
    {
        accountInfo = null;
        token = null;
        messageList = null;

        return true;
    }
}