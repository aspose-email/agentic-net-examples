using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP settings detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Determine the cutoff date (six months ago)
                    DateTime cutoffDate = DateTime.UtcNow.AddMonths(-6);

                    // Filter messages that are answered and older than six months
                    List<ImapMessageInfo> messagesToDelete = allMessages
                        .Where(msg => msg.Answered && msg.Date < cutoffDate)
                        .ToList();

                    if (messagesToDelete.Count == 0)
                    {
                        Console.WriteLine("No answered messages older than six months were found.");
                        return;
                    }

                    // Delete the filtered messages and commit the deletions immediately
                    client.DeleteMessages(messagesToDelete, true);
                    Console.WriteLine($"{messagesToDelete.Count} message(s) deleted successfully.");
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
