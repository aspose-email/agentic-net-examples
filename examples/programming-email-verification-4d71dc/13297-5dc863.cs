using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and credentials for the Exchange server
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client (returns an IEWSClient instance)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full mail message using its unique URI
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine("Subject: " + message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}