using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials/hosts
            if (exchangeUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Retrieve the URI of the default Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List all messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    Console.WriteLine($"Total messages retrieved: {messages.Count}");
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        string subject = messageInfo.Subject ?? "<no subject>";
                        Console.WriteLine($"- {subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
