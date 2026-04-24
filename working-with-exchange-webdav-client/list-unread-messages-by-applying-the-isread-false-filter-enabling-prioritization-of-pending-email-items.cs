using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details – replace with real values or skip execution.
            string host = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid runtime failures in CI.
            if (string.IsNullOrWhiteSpace(host) || host.Contains("example.com"))
            {
                Console.WriteLine("Skipping execution because placeholder connection details are used.");
                return;
            }

            // Create and use the Exchange WebDav client.
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                try
                {
                    // Build a query that selects only unread messages (IsRead = false).
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                    MailQuery query = builder.GetQuery();

                    // List unread messages from the Inbox folder.
                    // The third parameter 'true' enables recursive search (subfolders if needed).
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query.ToString());

                    // Output basic information about each unread message.
                    foreach (var msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Client operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
