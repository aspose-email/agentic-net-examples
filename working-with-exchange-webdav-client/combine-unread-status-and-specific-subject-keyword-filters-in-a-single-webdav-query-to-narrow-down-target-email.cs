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
            // Placeholder connection details – replace with real values when running against a server.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and dispose the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a query that selects unread messages with a specific subject keyword.
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                // Unread messages: messages that do NOT have the IsRead flag.
                builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                // Subject contains the keyword "Invoice".
                builder.Subject.Contains("Invoice");

                // Get the constructed MailQuery.
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (non‑recursive).
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query.ToString());

                // Output basic information about each matching message.
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}, IsRead: {messageInfo.IsRead}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
