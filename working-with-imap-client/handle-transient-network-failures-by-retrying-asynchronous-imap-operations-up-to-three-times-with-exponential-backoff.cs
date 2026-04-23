using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – real network calls are skipped.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host == "imap.example.com")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping real IMAP operations.");
                return;
            }

            // Create the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Attempt an asynchronous IMAP operation with retries.
                const int maxAttempts = 3;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        // Example operation: list messages in the INBOX folder.
                        // Replace with any other async IMAP method as needed.
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync("INBOX", CancellationToken.None);
                        Console.WriteLine($"Successfully retrieved {messages.Count} messages on attempt {attempt}.");
                        break; // Success – exit the retry loop.
                    }
                    catch (ImapException ex) when (attempt < maxAttempts)
                    {
                        // Transient failure – wait with exponential backoff before retrying.
                        int delaySeconds = (int)Math.Pow(2, attempt - 1);
                        Console.Error.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delaySeconds} second(s)...");
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                    }
                    catch (Exception ex)
                    {
                        // Non‑transient failure – report and exit.
                        Console.Error.WriteLine($"Operation failed: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
