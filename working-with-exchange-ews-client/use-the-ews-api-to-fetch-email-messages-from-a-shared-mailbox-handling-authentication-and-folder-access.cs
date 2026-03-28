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
            // Service URL and user credentials
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            // Email address of the shared mailbox
            string sharedMailbox = "shared@example.com";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Obtain mailbox information for the shared mailbox
                ExchangeMailboxInfo sharedInfo = client.GetMailboxInfo(sharedMailbox);

                // List messages in the Inbox of the shared mailbox
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(sharedInfo.InboxUri);

                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the full message using its unique URI
                    using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + (message.Subject ?? string.Empty));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
