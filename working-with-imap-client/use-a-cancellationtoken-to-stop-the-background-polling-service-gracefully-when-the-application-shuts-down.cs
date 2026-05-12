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
            // Placeholder credentials – skip actual network calls in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Wrap client connection in its own try/catch
                try
                {
                    // No explicit Connect call – operations will auto‑connect as needed
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to IMAP server: {ex.Message}");
                    return;
                }

                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        Console.WriteLine("Cancellation requested. Stopping polling...");
                        cts.Cancel();
                        e.Cancel = true; // Prevent immediate termination
                    };

                    // Start background polling
                    Task pollingTask = Task.Run(async () =>
                    {
                        while (!cts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                // Retrieve messages from INBOX
                                ImapMessageInfoCollection messages = await client.ListMessagesAsync("INBOX", cts.Token);
                                Console.WriteLine($"Fetched {messages.Count} messages at {DateTime.Now}.");
                            }
                            catch (OperationCanceledException)
                            {
                                // Expected on cancellation – exit loop
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error during polling: {ex.Message}");
                            }

                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(10), cts.Token);
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                        }
                    }, cts.Token);

                    // Wait for polling to finish (cancellation)
                    await pollingTask;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
