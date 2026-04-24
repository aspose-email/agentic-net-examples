using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main()
    {
        try
        {
            // Configuration (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string sourceFolder = "INBOX";
            string archiveFolder = "Archive";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP configuration detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the source folder
                    await client.SelectFolderAsync(sourceFolder, CancellationToken.None);

                    // Retrieve all messages in the folder
                    IEnumerable<ImapMessageInfo> messagesInfo = await client.ListMessagesAsync(CancellationToken.None);
                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message (processing placeholder)
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None);
                        Console.WriteLine($"Processing message: {message.Subject}");

                        // Archive the message by moving it to the archive folder
                        await client.MoveMessageAsync(info.UniqueId, archiveFolder, CancellationToken.None);

                        // Set the \Seen flag (IsRead) on the archived message
                        await client.AddMessageFlagsAsync(info.UniqueId, ImapMessageFlags.IsRead, CancellationToken.None);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
