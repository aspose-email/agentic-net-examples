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
            // Placeholder connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (mailboxUri.Contains("example") || username.Contains("example") || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange service connection.");
                return;
            }

            // Create the EWS client (Exchange service) inside a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Example operation: list the number of messages in the Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                    Console.WriteLine($"Inbox contains {messages.Count} messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
