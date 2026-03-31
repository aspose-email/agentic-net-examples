using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS";
            string username = "user@example.com";
            string password = "password";

            // Guard against real network calls with placeholder credentials
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Build a date filter for items on or after 2023-01-01
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.InternalDate.Since(new DateTime(2023, 1, 1));
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that satisfy the date filter
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    foreach (var messageInfo in messages)
                    {
                        // Fetch the full message to access its properties
                        MailMessage message = client.FetchMessage(messageInfo.UniqueUri);
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Dispose the fetched message
                        message.Dispose();
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
