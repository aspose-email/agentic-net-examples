using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // Placeholder credentials detection – skip real network calls in CI
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                    return;
                }

                // Connect to the IMAP server
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate the connection
                        client.ValidateCredentials();

                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve all messages in the folder
                        ImapMessageInfoCollection allMessages = client.ListMessages();

                        // Determine the cutoff date (messages older than one year)
                        DateTime cutoffDate = DateTime.UtcNow.AddYears(-1);

                        // Collect messages older than the cutoff date
                        List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                        foreach (ImapMessageInfo messageInfo in allMessages)
                        {
                            // InternalDate holds the received date of the message
                            if (messageInfo.InternalDate < cutoffDate)
                            {
                                messagesToDelete.Add(messageInfo);
                            }
                        }

                        // Perform batch deletion and commit immediately
                        if (messagesToDelete.Count > 0)
                        {
                            client.DeleteMessages(messagesToDelete, true);
                            Console.WriteLine($"{messagesToDelete.Count} messages older than one year were deleted.");
                        }
                        else
                        {
                            Console.WriteLine("No messages older than one year were found.");
                        }

                        // Verify removal by recounting messages in the folder
                        ImapMessageInfoCollection remainingMessages = client.ListMessages();
                        Console.WriteLine($"Remaining messages in INBOX: {remainingMessages.Count}");
                    }
                    catch (Exception ex)
                    {
                        // Friendly error handling for client operations
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
