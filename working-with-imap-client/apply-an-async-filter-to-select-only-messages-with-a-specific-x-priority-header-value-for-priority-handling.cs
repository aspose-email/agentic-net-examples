using Aspose.Email.Tools.Search;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials guard – do not attempt real network calls with dummy data.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Build a query to find messages with a specific X-Priority header value.
                    ImapQueryBuilder builder = new ImapQueryBuilder();
                    builder.HasHeader("X-Priority", "1"); // Adjust the value as needed.
                    MailQuery query = builder.GetQuery();

                    // Retrieve message infos that match the query from the INBOX folder.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(
                        "INBOX",
                        query,
                        int.MaxValue,
                        CancellationToken.None);

                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message to process it further if needed.
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None);
                        Console.WriteLine($"UID: {info.UniqueId}, Subject: {message.Subject}");
                        // Additional priority handling logic can be placed here.
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
