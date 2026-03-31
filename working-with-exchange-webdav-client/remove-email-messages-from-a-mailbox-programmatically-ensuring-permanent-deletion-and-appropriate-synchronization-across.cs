using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values when available.
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the Exchange client.
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                // Get the URI of the Inbox folder.
                string inboxUri = client.MailboxInfo.InboxUri;

                // Retrieve all messages from the Inbox.
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Destination folder for deleted items.
                string deletedItemsUri = client.MailboxInfo.DeletedItemsUri;

                // Iterate through each message and move it to Deleted Items.
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Move the message to the Deleted Items folder.
                    client.MoveMessage(messageInfo, deletedItemsUri);
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors.
            Console.Error.WriteLine(ex.Message);
        }
    }
}
