using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.gmail.com";
            string username = "your_email@gmail.com";
            string password = "your_password";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                username.Contains("your_") ||
                password.Contains("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Connect to Gmail via IMAP.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Ensure the label (folder) exists.
                    client.CreateFolder("ProjectX");

                    // Select the Inbox folder.
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the Inbox.
                    ImapMessageInfoCollection inboxMessages = client.ListMessages();

                    // Collect unique IDs of messages to move.
                    List<string> messageUids = new List<string>();
                    foreach (ImapMessageInfo info in inboxMessages)
                    {
                        // Example condition: move all messages (or add custom filter here).
                        messageUids.Add(info.UniqueId);
                    }

                    if (messageUids.Count > 0)
                    {
                        // Move the messages to the newly created label.
                        client.MoveMessages(messageUids, "ProjectX");
                        Console.WriteLine($"Moved {messageUids.Count} messages to label 'ProjectX'.");
                    }
                    else
                    {
                        Console.WriteLine("No messages found in INBOX to move.");
                    }
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
