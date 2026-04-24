using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used.
            if (string.IsNullOrWhiteSpace(host) ||
                host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Skipping IMAP operation due to placeholder credentials.");
                return;
            }

            // Create the mail message and add a custom header.
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Message with Custom Header";
                message.Body = "This is a test email sent via IAsyncImapClient.";

                // Add a custom header.
                message.Headers.Add("X-Custom-Header", "MyHeaderValue");

                // Connect to the IMAP server and upload the message.
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Optionally select a folder (e.g., "Sent") before appending.
                    // If omitted, the default folder will be used.
                    await client.SelectFolderAsync("Sent");

                    // Append the message asynchronously.
                    string uid = await client.AppendMessageAsync(message);
                    Console.WriteLine($"Message appended successfully. UID: {uid}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
