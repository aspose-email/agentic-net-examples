using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    // Entry point – async to avoid blocking the UI thread.
    static async Task Main(string[] args)
    {
        // Top‑level exception guard.
        try
        {
            // Placeholder credentials check – skip real network calls in CI.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping operation.");
                return;
            }

            // Compose a simple email message.
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email";
                message.Body = "This is a test email appended to the Sent folder.";

                // Create and use the IMAP client inside a using block.
                try
                {
                    using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                    {
                        // Validate credentials (asynchronous, wrapped in try/catch).
                        await client.ValidateCredentialsAsync();

                        // Append the message to the "Sent" folder asynchronously.
                        // AppendMessageAsync returns the UID of the created message.
                        string uid = await client.AppendMessageAsync("Sent", message);
                        Console.WriteLine($"Message appended successfully. UID: {uid}");
                    }
                }
                catch (Exception ex)
                {
                    // Friendly error output for client‑related failures.
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
