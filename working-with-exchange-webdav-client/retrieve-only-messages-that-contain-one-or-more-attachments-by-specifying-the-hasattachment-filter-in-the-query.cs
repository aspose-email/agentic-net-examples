using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls during CI
                if (string.IsNullOrWhiteSpace(exchangeUri) ||
                    exchangeUri.Contains("example.com") ||
                    username.Contains("example.com") ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                    return;
                }

                // Create and connect the Exchange WebDav client
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    try
                    {
                        // Build a query that filters messages having at least one attachment
                        ExchangeAdvancedSyntaxQueryBuilder queryBuilder = new ExchangeAdvancedSyntaxQueryBuilder();
                        // The HasAttachment field is a boolean; we compare it to true
                        queryBuilder.HasAttachment.Equals(true);
                        MailQuery query = queryBuilder.GetQuery();

                        // List messages from the Inbox that match the query
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query.ToString());

                        // Output subjects of messages that contain attachments
                        foreach (var messageInfo in messages)
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while querying messages: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return;
            }
        }
    }
}
