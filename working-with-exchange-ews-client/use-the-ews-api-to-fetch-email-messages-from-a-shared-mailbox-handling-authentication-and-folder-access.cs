using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // EWS endpoint of the shared mailbox
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                // Credentials of a user who has access to the shared mailbox
                NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

                // Create and dispose the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Retrieve messages from the Inbox folder of the shared mailbox
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                    foreach (ExchangeMessageInfo info in messageInfos)
                    {
                        // Fetch the full mail message for each item
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + message.Subject);
                            Console.WriteLine("From: " + (message.From != null ? message.From.Address : "Unknown"));
                            Console.WriteLine("Received: " + message.Date);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}