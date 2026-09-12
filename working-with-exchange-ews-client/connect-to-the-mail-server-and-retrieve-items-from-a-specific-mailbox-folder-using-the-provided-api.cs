using Aspose.Email.Mapi;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Get mailbox information to obtain the Inbox folder URI
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    string inboxUri = mailboxInfo.InboxUri;

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    Console.WriteLine($"Found {messages.Count} messages in Inbox.");

                    // Retrieve each message and display its subject
                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        // Fetch the full message as a MapiMessage
                        MapiMessage mapiMessage = client.FetchItem(msgInfo.UniqueUri);

                        // Convert to MailMessage for easy access to subject
                        MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Gracefully exit
                return;
            }
        }
    }
}
