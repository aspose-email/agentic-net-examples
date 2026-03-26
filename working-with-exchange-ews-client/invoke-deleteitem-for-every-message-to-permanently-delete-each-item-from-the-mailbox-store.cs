using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize the EWS client with placeholder credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List all messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Delete each message permanently
                foreach (ExchangeMessageInfo info in messages)
                {
                    client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}