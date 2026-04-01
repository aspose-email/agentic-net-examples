using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder values
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder connection parameters detected. Skipping Exchange client initialization.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Session settings
                    client.Timeout = 120000; // 2 minutes
                    client.UseDateInLogFileName = true;
                    client.LogFileName = "exchange_log.txt";

                    // Example operation: retrieve mailbox info
                    var mailboxInfo = client.MailboxInfo;
                    Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during client operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
