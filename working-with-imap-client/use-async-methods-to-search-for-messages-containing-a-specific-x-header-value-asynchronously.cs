using Aspose.Email.Tools.Search;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operation.");
                return;
            }

            // Build the search query for a specific X‑Header.
            ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
            MailQuery query = queryBuilder.HasHeader("X-Custom-Header", "DesiredValue");

            // Connect to the IMAP server and search asynchronously.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // List messages in INBOX that match the query.
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync(
                        folderName: "INBOX",
                        query: query,
                        maxNumberOfMessages: 0,
                        token: CancellationToken.None);

                    Console.WriteLine($"Found {messages.Count} message(s) with the specified X‑Header.");

                    foreach (ImapMessageInfo info in messages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {info.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
