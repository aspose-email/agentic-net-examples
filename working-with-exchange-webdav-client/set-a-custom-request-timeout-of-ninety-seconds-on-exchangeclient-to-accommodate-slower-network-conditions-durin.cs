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
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create ExchangeClient with a custom timeout of 90 seconds (90000 ms)
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                client.Timeout = 90000; // 90 seconds

                // List messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Iterate through the collection and output subject lines
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine(messageInfo.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
