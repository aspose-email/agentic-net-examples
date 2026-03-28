using Aspose.Email.Tools.Search;
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
            // Exchange server URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Build a query to find unread messages
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                MailQuery query = builder.GetQuery();

                // List unread messages from the Inbox folder
                ExchangeMessageInfoCollection infos = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Fetch and display each unread message
                foreach (ExchangeMessageInfo info in infos)
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
