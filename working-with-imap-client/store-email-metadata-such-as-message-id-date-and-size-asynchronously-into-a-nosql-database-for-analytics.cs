using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExample
{
    class Program
    {
        // Async entry point with top‑level exception guard
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder IMAP connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Skip real network calls when placeholders are used
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                    return;
                }

                // Create and use ImapClient inside a using block (client connection safety)
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Validate credentials by selecting the INBOX folder (lightweight operation)
                    try
                    {
                        await client.SelectFolderAsync("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                        return;
                    }

                    // Asynchronously list messages in the selected folder (no limit)
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync();

                    // Process each message metadata asynchronously
                    List<Task> storeTasks = new List<Task>();
                    foreach (ImapMessageInfo info in messages)
                    {
                        // Capture the current info for the async lambda
                        ImapMessageInfo currentInfo = info;
                        storeTasks.Add(StoreMetadataAsync(currentInfo));
                    }

                    // Wait for all storage operations to complete
                    await Task.WhenAll(storeTasks);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Simulated asynchronous storage of email metadata (Message‑Id, Date, Size)
        private static async Task StoreMetadataAsync(ImapMessageInfo info)
        {
            // In a real scenario, replace this with NoSQL DB client calls
            await Task.Run(() =>
            {
                Console.WriteLine($"Storing metadata -> MessageId: {info.MessageId}, Date: {info.Date}, Size: {info.Size} bytes");
            });
        }
    }
}
