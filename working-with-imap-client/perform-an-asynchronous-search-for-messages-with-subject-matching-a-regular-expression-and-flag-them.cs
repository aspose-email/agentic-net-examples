using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – execution skipped.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                // Simple validation by selecting the INBOX folder.
                client.SelectFolder("INBOX");

                // Build a search query that matches the subject using a regular expression.
                // The CustomSearch method allows raw IMAP search syntax.
                // Example regex: ".*Invoice.*" – adjust as needed.
                ImapQueryBuilder queryBuilder = new ImapQueryBuilder();
                MailQuery subjectQuery = queryBuilder.CustomSearch("SUBJECT \"*Invoice*\"");

                // Retrieve messages that satisfy the query.
                ImapMessageInfoCollection matchedMessages = await client.ListMessagesAsync(subjectQuery, CancellationToken.None);

                // Extract the unique IDs of the matched messages.
                List<string> uniqueIds = matchedMessages.Select(m => m.UniqueId).ToList();

                // Flag the matched messages (e.g., add the Flagged flag).
                if (uniqueIds.Count > 0)
                {
                    await client.AddMessageFlagsAsync(uniqueIds, ImapMessageFlags.Flagged);
                    Console.WriteLine($"{uniqueIds.Count} message(s) flagged.");
                }
                else
                {
                    Console.WriteLine("No messages matched the search criteria.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
