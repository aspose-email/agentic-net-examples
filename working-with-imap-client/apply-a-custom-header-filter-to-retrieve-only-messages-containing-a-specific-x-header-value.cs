using Aspose.Email;
using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping IMAP operation due to placeholder credentials.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a query that filters messages having a specific X‑Header value
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    queryBuilder.HasHeader("X-Custom-Header", "DesiredValue");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve messages matching the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Output subjects of the matching messages
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine(messageInfo.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("IMAP operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
