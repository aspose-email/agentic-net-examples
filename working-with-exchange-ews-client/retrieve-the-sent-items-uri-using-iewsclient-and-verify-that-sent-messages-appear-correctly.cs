using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSentItemsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve Sent Items folder URI
                    string sentItemsFolderUri = client.MailboxInfo.SentItemsUri;

                    // List messages in Sent Items folder
                    ExchangeMessageInfoCollection sentMessages = client.ListMessages(sentItemsFolderUri);

                    Console.WriteLine($"Total sent messages: {sentMessages.Count}");

                    // Verify each message by fetching and displaying its subject
                    foreach (ExchangeMessageInfo messageInfo in sentMessages)
                    {
                        // Fetch the full message using its unique URI
                        MailMessage fullMessage = client.FetchMessage(messageInfo.UniqueUri);
                        Console.WriteLine($"Subject: {fullMessage.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
