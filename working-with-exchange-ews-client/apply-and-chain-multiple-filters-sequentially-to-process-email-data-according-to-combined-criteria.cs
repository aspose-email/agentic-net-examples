using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details – replace with real values when available.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip actual network call when placeholders are detected.
                if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected – skipping EWS operations.");
                    return;
                }

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Build a composite query:
                    // 1. Messages received on or after a specific date (InternalDate).
                    // 2. Subject contains a keyword.
                    // 3. From address contains a domain.
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();

                    // Filter by internal date (received date) – using Since for >=.
                    DateTime startDate = DateTime.Today.AddDays(-30); // last 30 days
                    builder.InternalDate.Since(startDate);

                    // Subject contains "Report".
                    builder.Subject.Contains("Report");

                    // From contains "contoso.com".
                    builder.From.Contains("contoso.com");

                    // Generate the query.
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query.
                    // The Inbox URI is obtained from the mailbox info.
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                    // Iterate and display basic information.
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received (InternalDate): {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Graceful exit – do not rethrow.
            }
        }
    }
}
