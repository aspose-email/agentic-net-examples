using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder values are detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network call.");
                return;
            }

            // Create the IMAP client with SSL implicit security.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder asynchronously.
                    await client.SelectFolderAsync("INBOX");

                    // List messages in the selected folder.
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync();

                    // Fetch each message using BODY.PEEK (Aspose.Email fetch does not set the \Seen flag).
                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message without marking it as read.
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                        // Output basic information.
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    // Handle client operation errors.
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
