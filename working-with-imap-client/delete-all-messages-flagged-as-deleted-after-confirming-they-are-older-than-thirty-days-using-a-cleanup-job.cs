using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping cleanup job.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder.
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Identify messages flagged as Deleted and older than 30 days.
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    DateTime thresholdDate = DateTime.UtcNow.AddDays(-30);

                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        bool isDeleted = (messageInfo.Flags & ImapMessageFlags.Deleted) == ImapMessageFlags.Deleted;
                        bool isOld = messageInfo.InternalDate <= thresholdDate;

                        if (isDeleted && isOld)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    // Delete the identified messages if any.
                    if (messagesToDelete.Count > 0)
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} messages deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the cleanup criteria.");
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
