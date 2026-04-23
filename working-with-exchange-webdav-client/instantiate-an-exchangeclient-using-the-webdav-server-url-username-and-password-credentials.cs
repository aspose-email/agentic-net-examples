using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real server details.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing live network calls with placeholder data.
            if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Verify connectivity by accessing the Inbox folder.
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo(inboxUri);
                    Console.WriteLine($"Connected to Exchange. Inbox URI: {inboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to access mailbox: {ex.Message}");
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
