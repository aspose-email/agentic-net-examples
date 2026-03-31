using Aspose.Email.Clients.Exchange;
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
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Verify connection by retrieving mailbox info for the primary mailbox
                ExchangeMailboxInfo primaryInfo = client.GetMailboxInfo();
                Console.WriteLine($"Primary mailbox Inbox URI: {primaryInfo.InboxUri}");

                // Specify the shared mailbox to access
                string sharedMailbox = "shared@example.com";

                // Retrieve mailbox info for the shared mailbox
                ExchangeMailboxInfo sharedInfo = client.GetMailboxInfo(sharedMailbox);
                Console.WriteLine($"Shared mailbox Inbox URI: {sharedInfo.InboxUri}");

                // List messages from the shared mailbox's Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(sharedInfo.InboxUri);
                foreach (var msgInfo in messages)
                {
                    Console.WriteLine($"Subject: {msgInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
