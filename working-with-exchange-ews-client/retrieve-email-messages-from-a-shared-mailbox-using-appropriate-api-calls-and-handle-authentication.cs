using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sharedMailbox = "shared@example.com";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information for the shared mailbox
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo(sharedMailbox);
                string inboxUri = mailboxInfo.InboxUri;

                // List messages in the shared mailbox's Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message using its unique URI
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
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
