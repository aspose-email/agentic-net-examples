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
            // Initialize the EWS client with placeholder credentials.
            // Replace the URL, username, and password with real values when running the sample.
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", new NetworkCredential("username", "password")))
            {
                // Build a query that filters messages by subject containing the specified text.
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report"); // Subject criteria
                MailQuery query = builder.GetQuery();

                // Retrieve message infos from the Inbox that match the subject filter.
                ExchangeMessageInfoCollection infos = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                foreach (ExchangeMessageInfo info in infos)
                {
                    // Fetch the full message using its unique URI.
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
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
