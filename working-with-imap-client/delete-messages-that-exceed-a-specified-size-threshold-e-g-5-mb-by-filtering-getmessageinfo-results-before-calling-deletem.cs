using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapDeleteLargeMessages
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials detection – skip real network calls in CI environments.
                string host = "imap.example.com";
                string username = "username";
                string password = "password";

                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and dispose the IMAP client safely.
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Select the INBOX folder.
                        client.SelectFolder("INBOX");

                        // Retrieve all messages in the selected folder.
                        ImapMessageInfoCollection allMessages = client.ListMessages();

                        // Define the size threshold (5 MB).
                        long sizeThreshold = 5L * 1024L * 1024L;

                        // Collect messages that exceed the threshold.
                        List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                        foreach (ImapMessageInfo messageInfo in allMessages)
                        {
                            if (messageInfo.Size > sizeThreshold)
                            {
                                messagesToDelete.Add(messageInfo);
                            }
                        }

                        // Delete the large messages and commit the deletions.
                        if (messagesToDelete.Count > 0)
                        {
                            client.DeleteMessages(messagesToDelete, true);
                            Console.WriteLine($"{messagesToDelete.Count} messages larger than {sizeThreshold} bytes were deleted.");
                        }
                        else
                        {
                            Console.WriteLine("No messages exceed the size threshold.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during IMAP operations.
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
