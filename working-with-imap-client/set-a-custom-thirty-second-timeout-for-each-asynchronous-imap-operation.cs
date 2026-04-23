using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapTimeoutExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder IMAP server credentials.
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip real network calls when placeholders are used.
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Set a general timeout of 30 seconds (30000 ms) for client operations.
                    client.Timeout = 30000;

                    // Create a cancellation token source with a 30‑second timeout for each async call.
                    using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                    {
                        // Select the INBOX folder.
                        await client.SelectFolderAsync("INBOX", null, cts.Token);

                        // List messages in the selected folder.
                        var messages = await client.ListMessagesAsync(cts.Token);

                        Console.WriteLine($"Total messages in INBOX: {messages.Count}");

                        if (messages.Count > 0)
                        {
                            // Fetch the first message using its unique identifier.
                            var firstMessageInfo = messages[0];
                            var message = await client.FetchMessageAsync(firstMessageInfo.UniqueId, cts.Token);
                            Console.WriteLine($"Subject of first message: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
