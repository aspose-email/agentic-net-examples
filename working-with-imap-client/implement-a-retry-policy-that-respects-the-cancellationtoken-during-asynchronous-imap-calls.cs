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
            // Placeholder credentials – skip real network calls in CI
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                return;
            }

            // Create and dispose the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Cancellation token that can be triggered externally
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Example: list messages with retry policy
                    ImapMessageInfoCollection messages = await ExecuteWithRetryAsync(
                        token => client.ListMessagesAsync(10),
                        maxRetries: 3,
                        token: cts.Token);

                    Console.WriteLine($"Retrieved {messages?.Count ?? 0} messages.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Generic retry helper that respects CancellationToken
    private static async Task<T> ExecuteWithRetryAsync<T>(Func<CancellationToken, Task<T>> operation, int maxRetries, CancellationToken token)
    {
        int attempt = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                return await operation(token).ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                attempt++;
                if (attempt > maxRetries)
                    throw;

                // Simple back‑off delay respecting cancellation
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
            }
        }
    }
}
