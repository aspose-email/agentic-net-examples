using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize IMAP client (replace placeholders with actual values)
            using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Select the inbox folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Collect messages that match a criteria (e.g., subject contains "Test")
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    foreach (ImapMessageInfo info in allMessages)
                    {
                        if (!string.IsNullOrEmpty(info.Subject) && info.Subject.Contains("Test"))
                        {
                            messagesToDelete.Add(info);
                        }
                    }

                    if (messagesToDelete.Count > 0)
                    {
                        // Permanently delete the selected messages (commitNow = true)
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} message(s) permanently deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the deletion criteria.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during mailbox operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to initialize IMAP client: {ex.Message}");
        }
    }
}
