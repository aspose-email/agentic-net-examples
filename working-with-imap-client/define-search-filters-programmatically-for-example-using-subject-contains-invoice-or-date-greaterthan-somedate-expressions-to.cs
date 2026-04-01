using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailImapSearchExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against executing real network calls with placeholder credentials
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Build the search query: Subject contains "Invoice" AND SentDate within the last 30 days
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    DateTime thirtyDaysAgo = DateTime.Today.AddDays(-30);
                    builder.SentDate.Since(thirtyDaysAgo);

                    // Retrieve the combined query
                    MailQuery query = builder.GetQuery();

                    // Execute the search
                    ImapMessageInfoCollection messages = client.ListMessages(query);
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
