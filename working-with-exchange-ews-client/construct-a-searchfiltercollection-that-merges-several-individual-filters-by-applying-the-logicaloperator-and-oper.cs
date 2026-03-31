using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Build a query with multiple conditions (logical AND).
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report");
                builder.InternalDate.Greater(DateTime.Parse("1-Jan-2023"), DateComparisonType.ByDate);

                MailQuery query = builder.GetQuery();

                // List messages that satisfy all conditions.
                var messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                Console.WriteLine($"Matched {messages.Count} message(s).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
