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
            string host = "imap.example.com";
            int port = 993;
            bool useSsl = true;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password, useSsl))
            {
                // Build a query to filter messages where the subject contains a specific keyword (case‑insensitive)
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("keyword", true);
                MailQuery query = builder.GetQuery();

                // Retrieve messages matching the query
                Aspose.Email.Clients.Imap.ImapMessageInfoCollection messages = client.ListMessages(query);
                foreach (Aspose.Email.Clients.Imap.ImapMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
