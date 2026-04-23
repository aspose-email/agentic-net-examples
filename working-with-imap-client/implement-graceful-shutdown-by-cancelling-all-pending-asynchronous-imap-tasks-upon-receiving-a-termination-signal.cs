using Aspose.Email.Clients;
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
            string host = "imap.example.com";
            string username = "user";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                    Console.WriteLine("Cancellation requested via Ctrl+C.");
                };

                AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                {
                    cts.Cancel();
                    Console.WriteLine("Cancellation requested on process exit.");
                };

                try
                {
                    // Start an asynchronous IMAP operation.
                    Task listTask = client.ListMessagesAsync(100).ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            var messages = t.Result;
                            Console.WriteLine($"Retrieved {messages.Count} messages.");
                        }
                    }, cts.Token);

                    await listTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("IMAP operation was cancelled.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP error: {ex.Message}");
                }
                finally
                {
                    cts.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
