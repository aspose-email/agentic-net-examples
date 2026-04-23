using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server details – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Preserve the original ReadOnly state.
                    bool originalReadOnly = client.ReadOnly;

                    // Temporarily allow modifications.
                    client.ReadOnly = false;

                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder.
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Determine the cutoff date (30 days ago).
                    DateTime cutoffDate = DateTime.Now.AddDays(-30);

                    // Collect messages older than the cutoff.
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        // ImapMessageInfo.Date holds the internal date of the message.
                        if (messageInfo.Date < cutoffDate)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    // Delete the identified messages and commit the deletions.
                    if (messagesToDelete.Count > 0)
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} messages older than 30 days were deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages older than 30 days were found.");
                    }

                    // Restore the original ReadOnly mode.
                    client.ReadOnly = originalReadOnly;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
