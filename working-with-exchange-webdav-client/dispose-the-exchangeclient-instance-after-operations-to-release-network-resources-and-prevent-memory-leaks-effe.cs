using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (mailboxUri.Contains("example.com") || (username == "username" && password == "password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create and use the ExchangeClient within a using block to ensure disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Attempt to list messages in the Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                    Console.WriteLine($"Retrieved {messages.Count} messages from Inbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while accessing mailbox: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
