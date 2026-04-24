using Aspose.Email.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapAsyncFetch
{
    class Program
    {
        // Simple progress bar simulation in console
        private static void UpdateProgress(int percent)
        {
            Console.WriteLine($"Fetch progress: {percent}%");
        }

        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials - skip actual network call in CI
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                    return;
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Validate connection by selecting a folder (lightweight operation)
                    try
                    {
                        await client.SelectFolderAsync("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect or select folder: {ex.Message}");
                        return;
                    }

                    // Instantiate fetcher and subscribe to progress event
                    var fetcher = new MessageFetcher(client);
                    fetcher.ProgressChanged += (sender, percent) => UpdateProgress(percent);

                    // Fetch a message asynchronously (sequence number 1 as example)
                    await fetcher.FetchMessageAsync(1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    // Helper class that fetches a message and raises progress events
    class MessageFetcher
    {
        private readonly ImapClient _client;

        public MessageFetcher(ImapClient client)
        {
            _client = client;
        }

        // Event raised when fetch progress updates (0 to 100)
        public event EventHandler<int> ProgressChanged;

        // Asynchronously fetch a message by sequence number
        public async Task FetchMessageAsync(int sequenceNumber)
        {
            // Simulate progress: start
            OnProgressChanged(0);

            // Perform the actual fetch
            MailMessage message = await _client.FetchMessageAsync(sequenceNumber);

            // Simulate progress: complete
            OnProgressChanged(100);

            // Optionally process the fetched message (e.g., display subject)
            Console.WriteLine($"Fetched message subject: {message.Subject}");
        }

        protected virtual void OnProgressChanged(int percent)
        {
            ProgressChanged?.Invoke(this, percent);
        }
    }
}
