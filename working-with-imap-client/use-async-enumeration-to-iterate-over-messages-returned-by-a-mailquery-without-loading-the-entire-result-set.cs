using Aspose.Email.Tools.Search;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;


class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials check – skip execution if they are not real.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Build a simple MailQuery (e.g., messages from a specific address).
            MailQueryBuilder queryBuilder = new MailQueryBuilder();
            queryBuilder.From.Contains("example.com", ignoreCase: true);
            MailQuery query = queryBuilder.GetQuery();

            // Connect to the IMAP server.
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Asynchronously list messages that match the query.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(query, CancellationToken.None);

                    // Iterate over the result set without loading full messages.
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
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
