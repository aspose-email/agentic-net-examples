using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static async Task Main()
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP settings detected – skipping connection.");
                return;
            }

            // Create and connect the IMAP client inside a using block.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Wrap client operations in a try/catch to surface friendly errors.
                try
                {
                    // Asynchronously select the INBOX folder.
                    await client.SelectFolderAsync("INBOX");

                    // Asynchronously list message infos in the selected folder.
                    ImapMessageInfoCollection infos = await client.ListMessagesAsync();

                    // Prepare a list to hold fetched MailMessage objects.
                    List<MailMessage> messages = new List<MailMessage>();

                    // Fetch each message asynchronously without blocking the UI thread.
                    foreach (ImapMessageInfo info in infos)
                    {
                        // FetchMessageAsync returns a MailMessage for the given unique id.
                        MailMessage msg = await client.FetchMessageAsync(info.UniqueId);
                        messages.Add(msg);
                        Console.WriteLine($"Fetched: {msg.Subject}");
                    }

                    // Example: process the messages (here we just output the count).
                    Console.WriteLine($"Total messages fetched: {messages.Count}");
                }
                catch (Exception ex)
                {
                    // Friendly error handling for client operations.
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Global exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
