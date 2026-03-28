using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
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
            // Initialize the EWS client
            IEWSClient client = null;
            try
            {
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(serviceUrl, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Build a query to find unread messages
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                MailQuery query = builder.GetQuery();

                // List unread messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, true);

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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
