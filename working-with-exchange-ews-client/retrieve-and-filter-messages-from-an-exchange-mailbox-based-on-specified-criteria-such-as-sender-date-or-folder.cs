using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;
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
            // Placeholder connection data – replace with real values or keep as is for a safe early exit.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username   = "user@example.com";
            string password   = "password";

            // Guard against executing with placeholder credentials.
            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping live Exchange connection.");
                return;
            }

            // Create the EWS client inside a using block to ensure proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Build a query to filter messages by sender and sent date (last 7 days).
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.From.Contains("sender@example.com");
                    builder.InternalDate.Since(DateTime.Today.AddDays(-7));

                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that match the query.
                    ExchangeMessageInfoCollection infos = client.ListMessages(
                        client.MailboxInfo.InboxUri,
                        query); // non‑recursive

                    // Process the resulting messages.
                    foreach (ExchangeMessageInfo info in infos)
                    {
                        // Fetch the full message to access its properties (e.g., Subject).
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Dispose the fetched message.
                        message.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
