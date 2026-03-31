using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("@example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create the Exchange WebDAV client inside a using block to ensure disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a complex query:
                //   From contains "alice@example.com"
                //   AND Subject contains "Report"
                //   AND InternalDate is on or after 1 Jan 2023
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.From.Contains("alice@example.com");
                builder.Subject.Contains("Report");
                builder.InternalDate.Since(new DateTime(2023, 1, 1));

                // Get the MailQuery object from the builder
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (recursive search)
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,   // folder URI
                    query,                         // search criteria
                    true);                         // recursive

                // Output basic information about each matched message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {string.Join(", ", info.From)}");
                    Console.WriteLine($"Internal Date: {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard – report errors without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
