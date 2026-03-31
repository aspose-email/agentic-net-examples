using System;
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
            // Placeholder connection details – replace with real values when running against a live server
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external call if placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server interaction.");
                return;
            }

            // Initialize the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a query:
                //   - Sent date on or after 1 Jan 2023
                //   - Subject contains the word "Report"
                DateTime startDate = new DateTime(2023, 1, 1);
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.SentDate.Since(startDate);
                builder.Subject.Contains("Report");

                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query (non‑recursive)
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, false);

                // Output basic information for each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"Sent: {info.Date}");
                    Console.WriteLine($"URI: {info.UniqueUri}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
