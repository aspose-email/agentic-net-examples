using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Mailbox URI, username, or password not provided.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Build a query to filter messages where the sender's address contains the domain "example.com"
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.From.Contains("example.com");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query.ToString());
                    foreach (var messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
