using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network call in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping deletion operation.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Select the folder containing the messages (e.g., INBOX)
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Prepare a batch of messages to delete (first 10 messages, if available)
                    List<ImapMessageInfo> batch = new List<ImapMessageInfo>();
                    int count = 0;
                    foreach (ImapMessageInfo info in allMessages)
                    {
                        batch.Add(info);
                        count++;
                        if (count >= 10)
                            break;
                    }

                    if (batch.Count == 0)
                    {
                        Console.WriteLine("No messages found to delete.");
                        return;
                    }

                    // Delete the batch of messages
                    client.DeleteMessages(batch);

                    // Commit the deletions on the server
                    Console.WriteLine($"{batch.Count} messages have been deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
