using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls in CI
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping server operations.");
                return;
            }

            // Define the cutoff date; messages older than this will be deleted
            DateTime cutoffDate = new DateTime(2023, 1, 1);

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Validate credentials before proceeding
                    if (!client.ValidateCredentials())
                    {
                        Console.Error.WriteLine("IMAP authentication failed.");
                        return;
                    }

                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Collect messages older than the cutoff date
                    List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();
                    foreach (ImapMessageInfo messageInfo in allMessages)
                    {
                        if (messageInfo.InternalDate < cutoffDate)
                        {
                            messagesToDelete.Add(messageInfo);
                        }
                    }

                    // Delete the selected messages and commit the deletions
                    if (messagesToDelete.Count > 0)
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} messages older than {cutoffDate:d} were deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages found older than the specified date.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
