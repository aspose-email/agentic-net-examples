using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if they are not replaced.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("IMAP credentials are placeholders. Skipping execution.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder (lightweight operation to validate connection).
                    client.SelectFolder("INBOX");

                    // Calculate the date threshold (messages older than one year).
                    DateTime threshold = DateTime.UtcNow.AddYears(-1);

                    // Retrieve all messages in the selected folder.
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Collect messages older than the threshold.
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        if (messageInfo.InternalDate < threshold)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    // Delete the collected messages and commit the deletions.
                    if (messagesToDelete.Count > 0)
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} message(s) older than one year were deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages older than one year were found.");
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
