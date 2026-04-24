using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            RunAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static async Task RunAsync()
    {
        // Placeholder credentials guard – skip real network calls when using example data.
        string host = "imap.example.com";
        string username = "user@example.com";
        string password = "password";

        if (host.Contains("example.com"))
        {
            Console.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
            return;
        }

        // Connect to IMAP server and retrieve message list.
        try
        {
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Select the folder to work with.
                client.SelectFolder("INBOX");

                // Retrieve a collection of message infos (adjust the max count as needed).
                ImapMessageInfoCollection allInfos = await client.ListMessagesAsync(1000);

                // Extract sequence numbers for fetching.
                List<int> sequenceNumbers = allInfos.Select(info => info.SequenceNumber).ToList();

                // Define chunk size for parallel processing.
                const int chunkSize = 10;
                var chunks = sequenceNumbers
                    .Select((num, index) => new { num, index })
                    .GroupBy(x => x.index / chunkSize, x => x.num)
                    .Select(g => g.ToList())
                    .ToList();

                // Prepare parallel fetch tasks.
                List<Task> fetchTasks = new List<Task>();

                foreach (List<int> chunk in chunks)
                {
                    fetchTasks.Add(Task.Run(async () =>
                    {
                        // Each task uses its own client instance.
                        using (ImapClient innerClient = new ImapClient(host, username, password))
                        {
                            innerClient.SelectFolder("INBOX");
                            IList<MailMessage> messages = await innerClient.FetchMessagesAsync(chunk);

                            // Example processing: output subject lines.
                            foreach (MailMessage msg in messages)
                            {
                                Console.WriteLine($"Subject: {msg.Subject}");
                            }
                        }
                    }));
                }

                // Await all parallel fetches.
                await Task.WhenAll(fetchTasks);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
        }
    }
}
