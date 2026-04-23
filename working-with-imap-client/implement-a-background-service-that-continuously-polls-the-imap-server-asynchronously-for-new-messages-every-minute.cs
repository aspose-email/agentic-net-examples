using System;
using System.Collections.Generic;
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
            await RunImapPollingServiceAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task RunImapPollingServiceAsync()
    {
        // Placeholder credentials – skip real network calls in CI environments
        const string host = "imap.example.com";
        const string username = "user@example.com";
        const string password = "password";

        if (host.Contains("example.com"))
        {
            Console.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
            return;
        }

        // Track already processed message UIDs to avoid duplicates
        HashSet<string> processedUids = new HashSet<string>();

        // Cancellation token to stop the background loop gracefully (e.g., on Ctrl+C)
        using (CancellationTokenSource cts = new CancellationTokenSource())
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Cancellation requested...");
            };

            // Main polling loop
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    // Create and use the IMAP client inside a using block
                    using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                    {
                        // Select the INBOX folder
                        await client.SelectFolderAsync("INBOX", null, cts.Token);

                        // Retrieve the most recent message (you can adjust the number as needed)
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync(1, cts.Token);

                        foreach (ImapMessageInfo info in messages)
                        {
                            // Use the unique identifier to detect new messages
                            if (!processedUids.Contains(info.UniqueId))
                            {
                                // Fetch the full message
                                MailMessage message = await client.FetchMessageAsync(info.UniqueId, cts.Token);
                                Console.WriteLine($"New message received: Subject = {message.Subject}");

                                // Mark as processed
                                processedUids.Add(info.UniqueId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log client‑related errors but continue the loop
                    Console.Error.WriteLine($"IMAP polling error: {ex.Message}");
                }

                // Wait for one minute before the next poll
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), cts.Token);
                }
                catch (TaskCanceledException)
                {
                    // Exit loop if cancellation was requested during the delay
                    break;
                }
            }
        }
    }
}
