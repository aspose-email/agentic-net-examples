using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

namespace DeleteBatchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize IMAP client with connection settings
            ImapClient imapClient = new ImapClient
            {
                Host = "imap.example.com",
                Port = 993,
                SecurityOptions = SecurityOptions.SSLImplicit,
                Username = "user@example.com",
                Password = "password"
            };

            // Guard: skip real network calls when placeholder credentials are present
            if (imapClient.Host.Contains("example.com") ||
                imapClient.Username.Contains("example.com") ||
                imapClient.Password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Use the client within a using block to ensure proper disposal
            using (imapClient)
            {
                try
                {
                    // Select the INBOX folder
                    imapClient.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = imapClient.ListMessages();

                    // Prepare a batch of messages to delete (e.g., first 5 messages)
                    ImapMessageInfoCollection messagesToDelete = new ImapMessageInfoCollection();
                    int batchSize = 5;
                    for (int i = 0; i < batchSize && i < allMessages.Count; i++)
                    {
                        messagesToDelete.Add(allMessages[i]);
                    }

                    if (messagesToDelete.Count > 0)
                    {
                        // Delete the selected batch of messages
                        imapClient.DeleteMessages(messagesToDelete);
                        Console.WriteLine($"{messagesToDelete.Count} messages marked as deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages found to delete.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
    }
}
