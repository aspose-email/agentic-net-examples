using Aspose.Email.Tools.Search;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (serverUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the Exchange client
            try
            {
                using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
                {
                    // Define the recipient to filter messages for
                    string recipient = "alice@example.com";

                    // Build a query that selects messages addressed to the specified recipient
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.To.Contains(recipient);
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that match the query (recursive = false)
                    ExchangeMessageInfoCollection messages = client.ListMessages(
                        client.MailboxInfo.InboxUri,
                        query,
                        false);

                    // Process the filtered messages
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"To: {info.To}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
