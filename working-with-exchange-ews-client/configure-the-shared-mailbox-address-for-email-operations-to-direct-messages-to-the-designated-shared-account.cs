using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with server URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            NetworkCredential credentials = new NetworkCredential(username, password);
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Define the shared mailbox address
                string sharedMailbox = "shared@example.com";

                // Retrieve mailbox information for the shared mailbox
                ExchangeMailboxInfo sharedInfo = client.GetMailboxInfo(sharedMailbox);

                // List messages from the shared mailbox's Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sharedInfo.InboxUri);
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Fetch the full mail message using its unique URI
                    MailMessage message = client.FetchMessage(messageInfo.UniqueUri);
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
