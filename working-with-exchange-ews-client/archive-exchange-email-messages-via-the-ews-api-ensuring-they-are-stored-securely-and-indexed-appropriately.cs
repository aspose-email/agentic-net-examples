using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace ArchiveEwsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the EWS endpoint and credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Get the Inbox folder URI.
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List all messages in the Inbox.
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    // Archive each message.
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        client.ArchiveItem(inboxUri, messageInfo.UniqueUri);
                        Console.WriteLine($"Archived message: {messageInfo.Subject}");
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
