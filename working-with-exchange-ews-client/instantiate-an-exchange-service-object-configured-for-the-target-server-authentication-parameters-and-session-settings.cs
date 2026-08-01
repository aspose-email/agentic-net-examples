using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters – replace with real values.
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Session settings.
                client.Timeout = 120000; // 2 minutes.
                client.UseDateInLogFileName = true;
                client.LogFileName = "ews_log.txt";

                // Example operation: retrieve mailbox information.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
