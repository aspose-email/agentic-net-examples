using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService; // for IEWSClient and EWSClient
using Aspose.Email.Clients.Exchange; // for ExchangeMessageInfo

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI of the shared mailbox (e.g., https://exchange.example.com/EWS/Exchange.asmx)
            string sharedMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Credentials for the user who has access to the shared mailbox
            ICredentials credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(sharedMailboxUri, credentials))
            {
                // Optionally, set the mailbox URI explicitly to the shared mailbox
                client.MailboxUri = sharedMailboxUri;

                // Retrieve mailbox information (contains folder URIs)
                var mailboxInfo = client.MailboxInfo;

                // List messages in the Inbox folder of the shared mailbox
                var messages = client.ListMessages(mailboxInfo.InboxUri);

                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                {
                    // Output basic metadata
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + (info.From != null ? info.From[0].DisplayName : "Unknown"));
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));

                    // Fetch the full MailMessage if needed
                    MailMessage fullMessage = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine("Full Message Subject: " + fullMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}