using Aspose.Email;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls if they are not replaced.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operations.");
                return;
            }

            // Create and configure the IMAP client.
            using (ImapClient client = new ImapClient(host, 993, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                // Cancellation token source to abort pending async operations.
                using (CancellationTokenSource cancellationSource = new CancellationTokenSource())
                {
                    // Start a background task that monitors connectivity.
                    Task monitorTask = MonitorConnectivityAsync(client, cancellationSource);

                    // Example asynchronous IMAP operation.
                    try
                    {
                        // List a limited number of messages in the selected folder.
                        // This overload does not accept a token; cancellation will be triggered by the monitor.
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync(10);
                        Console.WriteLine($"Fetched {messages.Count} messages.");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.Error.WriteLine("IMAP operation was canceled due to connectivity loss.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
                    }

                    // Ensure the monitor stops.
                    cancellationSource.Cancel();
                    await monitorTask;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Periodically sends a NOOP command; cancels the token source if the server is unreachable.
    private static async Task MonitorConnectivityAsync(ImapClient client, CancellationTokenSource cancellationSource)
    {
        while (!cancellationSource.Token.IsCancellationRequested)
        {
            try
            {
                await client.NoopAsync(null, cancellationSource.Token);
            }
            catch
            {
                // Connectivity lost – trigger cancellation of all pending tasks.
                cancellationSource.Cancel();
                break;
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested; exit loop.
                break;
            }
        }
    }
}
