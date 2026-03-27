using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine("Subject: " + messageInfo.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
