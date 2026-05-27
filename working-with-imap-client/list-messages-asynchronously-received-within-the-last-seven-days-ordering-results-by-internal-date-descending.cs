using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        // Entry point
        static async Task Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder credentials check – avoid real network calls in CI
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder IMAP server/credentials detected – skipping execution.");
                    return;
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
                {
                    client.Username = username;
                    client.Password = password;

                    // Build a query for messages received in the last 7 days
                    ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                    DateTime sinceDate = DateTime.UtcNow.AddDays(-7);
                    MailQuery dateQuery = queryBuilder.InternalDate.Since(sinceDate);

                    // Retrieve messages from INBOX matching the query
                    // Using the overload that accepts folder name, query and max number of messages
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(
                        "INBOX",
                        dateQuery,
                        int.MaxValue);

                    // Order the messages by internal date descending
                    IEnumerable<ImapMessageInfo> orderedInfos = messageInfos
                        .OrderByDescending(info => info.InternalDate);

                    // Display basic information for each message
                    foreach (ImapMessageInfo info in orderedInfos)
                    {
                        Console.WriteLine($"Date: {info.InternalDate:u} | From: {info.From} | Subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Friendly error output
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
