using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                Console.WriteLine("Total messages in Inbox: " + messages.Count);

                // Mark the first message as read (if any)
                if (messages.Count > 0)
                {
                    ExchangeMessageInfo firstMessage = messages[0];
                    client.SetReadFlag(firstMessage.UniqueUri, true);
                    Console.WriteLine("Marked message as read: " + firstMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
