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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sharedMailbox = "shared@example.com";

            // Guard against executing live calls with placeholder data
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping live EWS call.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve mailbox info for the shared mailbox
                    ExchangeMailboxInfo sharedInfo = client.GetMailboxInfo(sharedMailbox);
                    string inboxUri = sharedInfo.InboxUri;
                    Console.WriteLine($"Shared mailbox Inbox URI: {inboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving shared mailbox info: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
