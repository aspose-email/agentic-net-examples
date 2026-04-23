using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

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
            if (host.Equals("imap.example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("user@example.com", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password))
            {
                // Select the INBOX folder
                imapClient.SelectFolder("INBOX");

                // Retrieve all messages in the folder
                ImapMessageInfoCollection allMessages = imapClient.ListMessages();

                // Determine the cutoff date (messages older than 60 days)
                DateTime cutoffDate = DateTime.Now.AddDays(-60);

                // Collect messages that are flagged and older than the cutoff
                List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                foreach (ImapMessageInfo messageInfo in allMessages)
                {
                    if (messageInfo.Flagged && messageInfo.InternalDate < cutoffDate)
                    {
                        messagesToDelete.Add(messageInfo);
                    }
                }

                // Delete the identified messages and commit the deletions
                if (messagesToDelete.Count > 0)
                {
                    imapClient.DeleteMessages(messagesToDelete, true);
                    Console.WriteLine($"{messagesToDelete.Count} flagged messages older than 60 days were deleted.");
                }
                else
                {
                    Console.WriteLine("No flagged messages older than 60 days were found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
