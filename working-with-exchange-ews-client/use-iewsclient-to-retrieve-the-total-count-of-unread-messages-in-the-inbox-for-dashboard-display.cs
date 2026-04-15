using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Obtain the Inbox folder URI from the mailbox information
                    string inboxFolderUri = client.MailboxInfo.InboxUri;

                    // Retrieve folder information for the Inbox
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo(inboxFolderUri);

                    // Get the unread messages count
                    int unreadCount = inboxInfo.UnreadCount;

                    Console.WriteLine($"Unread messages in Inbox: {unreadCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving unread count: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
