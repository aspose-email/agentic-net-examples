using Aspose.Email.Clients;
using System;
using System.Text;
using System.Threading;
using Aspose.Email;
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
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the folder to search
                    client.SelectFolder("INBOX");

                    // Build a complex search query using ImapQueryBuilder
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    // Example criteria: Subject contains "Report" AND From contains "sender@example.com"
                    queryBuilder.Subject.Contains("Report");
                    queryBuilder.From.Contains("sender@example.com");
                    // Convert builder to MailQuery
                    MailQuery query = queryBuilder.GetQuery();

                    // Execute the search (retrieve up to 100 messages)
                    ImapMessageInfoCollection messages = client.ListMessagesAsync(query, 100, CancellationToken.None).Result;

                    // Output basic information about each found message
                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"UID: {info.UniqueId}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
}
