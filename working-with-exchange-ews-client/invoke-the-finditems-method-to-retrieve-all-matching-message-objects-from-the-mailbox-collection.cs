using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client inside a using block to ensure disposal.
                using (IEWSClient client = EWSClient.GetEWSClient(
                    "https://example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password")))
                {
                    try
                    {
                        // Build a query to find messages (example: subject contains "Report").
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Report");
                        MailQuery query = builder.GetQuery();

                        // Retrieve message infos from the Inbox folder matching the query.
                        ExchangeMessageInfoCollection messageInfos = client.ListMessages(
                            client.MailboxInfo.InboxUri,
                            query,
                            false);

                        // Iterate through each message info, fetch the full message, and display its subject.
                        foreach (ExchangeMessageInfo info in messageInfos)
                        {
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during EWS operations: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
