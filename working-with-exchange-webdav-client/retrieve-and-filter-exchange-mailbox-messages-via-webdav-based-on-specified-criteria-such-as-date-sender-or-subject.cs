using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

namespace AsposeEmailWebDavExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and use the Exchange WebDAV client inside a using block
                try
                {
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                    {
                        // Build a query to filter messages
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();

                        // Example: messages sent in the last 30 days
                        DateTime fromDate = DateTime.Now.AddDays(-30);
                        builder.SentDate.Since(fromDate);

                        // Example: messages from a specific sender
                        builder.From.Contains("alice@example.com");

                        // Example: messages with a subject containing a keyword
                        builder.Subject.Contains("Report");

                        // Get the constructed query
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages from the Inbox that match the query (non‑recursive)
                        string inboxUri = client.MailboxInfo.InboxUri;
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query, false);

                        // Process the retrieved messages
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine("Subject: {0}", messageInfo.Subject);
                            Console.WriteLine("From   : {0}", messageInfo.From);
                            Console.WriteLine("Date   : {0}", messageInfo.Date);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle client creation or operation errors
                    Console.Error.WriteLine("Error accessing Exchange server: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
