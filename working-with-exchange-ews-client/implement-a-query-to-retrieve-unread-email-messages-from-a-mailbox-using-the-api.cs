using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox URI and credentials (replace with actual values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a query to find unread messages (IsRead flag not set)
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                MailQuery query = builder.GetQuery();

                // List unread messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Iterate through the results and fetch each full message
                foreach (ExchangeMessageInfo info in messages)
                {
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine("Subject: " + message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
