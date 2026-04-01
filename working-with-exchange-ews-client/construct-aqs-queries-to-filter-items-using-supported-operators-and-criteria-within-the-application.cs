using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AqsQuerySample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls in CI.
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // Create the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Build an AQS query: subject contains "report" AND from contains "example@domain.com".
                        ExchangeAdvancedSyntaxQueryBuilder builder = new ExchangeAdvancedSyntaxQueryBuilder();
                        builder.Subject.Contains("report");
                        builder.From.Contains("example@domain.com");
                        MailQuery query = builder.GetQuery();

                        // List messages from the Inbox that match the query.
                        string inboxUri = client.MailboxInfo.InboxUri;
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                        // Output the subjects of the retrieved messages.
                        foreach (var messageInfo in messages)
                        {
                            Console.WriteLine($"Message URI: {messageInfo.UniqueUri}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
