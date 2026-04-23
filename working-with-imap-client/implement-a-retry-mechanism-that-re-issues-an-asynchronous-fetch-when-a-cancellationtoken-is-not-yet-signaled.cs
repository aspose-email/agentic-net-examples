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
            // Placeholder credentials – skip real network call in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operation.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Example message UID to fetch
                string messageUid = "12345";

                // Cancellation token that can be cancelled externally
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Attempt to fetch with up to 3 retries while token is not cancelled
                    MailMessage message = await FetchMessageWithRetryAsync(client, messageUid, cts.Token, maxAttempts: 3);
                    if (message != null)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                    else
                    {
                        Console.WriteLine("Message could not be fetched.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task<MailMessage> FetchMessageWithRetryAsync(ImapClient client, string uid, CancellationToken token, int maxAttempts)
    {
        int attempt = 0;
        while (attempt < maxAttempts && !token.IsCancellationRequested)
        {
            try
            {
                // Asynchronously fetch the message by UID
                MailMessage msg = await client.FetchMessageAsync(uid, token);
                return msg; // Success
            }
            catch (ImapException imapEx)
            {
                attempt++;
                Console.Error.WriteLine($"Attempt {attempt} failed: {imapEx.Message}");
                if (attempt >= maxAttempts || token.IsCancellationRequested)
                {
                    break;
                }
                // Optional: small delay before retry
                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
            catch (Exception ex)
            {
                // Non‑IMAP exceptions are not retried
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                break;
            }
        }
        return null;
    }
}
