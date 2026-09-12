using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapPreviewExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholders are detected to avoid external calls during CI.
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create a cancellation token (could be linked to a timeout if needed).
                CancellationToken cancellationToken = CancellationToken.None;

                // Use the ImapClient inside a using block to ensure proper disposal.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the INBOX folder (optional, ListMessagesAsync works on the selected folder).
                    client.SelectFolder("INBOX");

                    // Retrieve up to ten messages from the server.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(10, cancellationToken);

                    // Prepare a list to hold the unseen messages.
                    List<MailMessage> unseenMessages = new List<MailMessage>();

                    // Iterate through the fetched message infos.
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Check if the message has not been read.
                        if (!info.IsRead)
                        {
                            // Fetch the full message asynchronously using its unique ID.
                            MailMessage message = await client.FetchMessageAsync(info.UniqueId, cancellationToken);
                            unseenMessages.Add(message);

                            // Stop after collecting ten unseen messages.
                            if (unseenMessages.Count >= 10)
                                break;
                        }
                    }

                    // Output a quick preview of the unseen messages.
                    Console.WriteLine($"Found {unseenMessages.Count} unseen message(s) (preview of up to 10):");
                    foreach (MailMessage msg in unseenMessages)
                    {
                        Console.WriteLine($"- Subject: {msg.Subject}");
                        Console.WriteLine($"  From: {msg.From}");
                        Console.WriteLine($"  Date: {msg.Date}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                // Gracefully handle any unexpected errors.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
