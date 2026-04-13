using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
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

                // Skip execution when placeholders are detected
                if (mailboxUri.Contains("example.com") || username == "username")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                    return;
                }

                // Create and use the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Get the Inbox folder URI
                        string inboxUri = client.MailboxInfo.InboxUri;

                        // Retrieve messages from the Inbox
                        ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);

                        // Report subject and sender for each message
                        foreach (ExchangeMessageInfo messageInfo in messageInfos)
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            Console.WriteLine($"Sender: {messageInfo.Sender}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
