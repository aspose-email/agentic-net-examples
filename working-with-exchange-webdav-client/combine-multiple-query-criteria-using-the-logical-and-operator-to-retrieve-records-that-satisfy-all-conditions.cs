using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Placeholder credentials – skip actual network call in CI
            const string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            const string username = "username";
            const string password = "password";

            if (username == "username" && password == "password")
            {
                Console.WriteLine("Placeholder credentials detected – execution skipped.");
                return;
            }

            // Client connection safety guard
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Build a query that combines multiple criteria with logical AND
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.From.Contains("alice@example.com");
                    builder.Subject.Contains("Report");

                    // The query returned by GetQuery() represents the AND of all added criteria
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that satisfy both conditions
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, true);

                    // Output basic information for each matching message
                    foreach (var msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject} | From: {msgInfo.From}");
                    }
                }
                catch (Exception ex)
                {
                    // Friendly error output for client operations
                    Console.Error.WriteLine($"Client error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Global error handling
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
