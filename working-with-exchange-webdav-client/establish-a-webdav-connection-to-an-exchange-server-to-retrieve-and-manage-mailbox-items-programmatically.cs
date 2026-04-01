using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when using placeholder values
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Connect to Exchange using WebDAV (ExchangeClient)
            using (ExchangeClient client = new ExchangeClient(mailboxUri, credentials))
            {
                try
                {
                    // List messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate over the collection
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                        Console.WriteLine($"Received: {messageInfo.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
