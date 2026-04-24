using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    DateTime cutoffDate = DateTime.UtcNow.AddDays(-90);

                    // Filter messages that are marked as Seen (IsRead) and older than 90 days
                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        if (messageInfo.IsRead && messageInfo.InternalDate < cutoffDate)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    if (messagesToDelete.Count > 0)
                    {
                        // Delete the selected messages and commit immediately
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} old read messages deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No old read messages found.");
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
