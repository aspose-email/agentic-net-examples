using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials and EWS URL for the shared mailbox
            string ewsUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
            {
                // Get the Inbox URI of the shared mailbox
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection messagesInfo = client.ListMessages(inboxUri);

                // Iterate through each message info and fetch the full message
                foreach (ExchangeMessageInfo info in messagesInfo)
                {
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
