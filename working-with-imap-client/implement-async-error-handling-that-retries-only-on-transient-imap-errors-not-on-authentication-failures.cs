using System;
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
            // Placeholder guard – skip real network calls when using example credentials.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                password == "password")
            {
                Console.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    await ListMessagesWithRetryAsync(client);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Performs ListMessagesAsync with retry logic for transient ImapException errors.
    private static async Task ListMessagesWithRetryAsync(ImapClient client)
    {
        const int maxRetries = 3;
        const int delayMilliseconds = 2000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // Retrieve up to 10 messages from the currently selected folder.
                ImapMessageInfoCollection messages = await client.ListMessagesAsync(10);
                Console.WriteLine($"Retrieved {messages.Count} messages.");
                foreach (var info in messages)
                {
                    Console.WriteLine($"- UID: {info.UniqueId}, Subject: {info.Subject}");
                }
                break; // Success, exit retry loop.
            }
            catch (ImapException imapEx)
            {
                // Do not retry on authentication failures.
                if (imapEx.Message.IndexOf("authentication", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.Error.WriteLine($"Authentication failure: {imapEx.Message}");
                    return;
                }

                if (attempt == maxRetries)
                {
                    Console.Error.WriteLine($"Operation failed after {maxRetries} attempts: {imapEx.Message}");
                    return;
                }

                Console.Error.WriteLine($"Transient IMAP error (attempt {attempt}): {imapEx.Message}");
                await Task.Delay(delayMilliseconds);
            }
        }
    }
}
