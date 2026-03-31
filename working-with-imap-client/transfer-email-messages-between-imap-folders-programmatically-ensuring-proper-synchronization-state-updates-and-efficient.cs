using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string sourceFolder = "INBOX";
            string destinationFolder = "Processed";

            // Skip real network calls when placeholders are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Ensure the destination folder exists; create if it does not
                    if (!client.ExistFolder(destinationFolder))
                    {
                        client.CreateFolder(destinationFolder);
                    }

                    // Select the source folder
                    client.SelectFolder(sourceFolder);

                    // Retrieve list of messages in the source folder
                    IList<ImapMessageInfo> messages = client.ListMessages();

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages to transfer.");
                        return;
                    }

                    // Collect unique identifiers of messages to move
                    List<string> messageIds = new List<string>();
                    foreach (ImapMessageInfo info in messages)
                    {
                        messageIds.Add(info.UniqueId);
                    }

                    // Move messages to the destination folder
                    client.MoveMessages(messageIds, destinationFolder);

                    Console.WriteLine($"Transferred {messageIds.Count} messages from '{sourceFolder}' to '{destinationFolder}'.");
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
