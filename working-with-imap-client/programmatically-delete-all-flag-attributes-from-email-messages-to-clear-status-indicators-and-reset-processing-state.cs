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
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    IList<ImapMessageInfo> messages = client.ListMessages();

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the folder.");
                        return;
                    }

                    // Combine all possible flags to be removed
                    ImapMessageFlags flagsToRemove = ImapMessageFlags.Answered |
                                                      ImapMessageFlags.Deleted |
                                                      ImapMessageFlags.Draft |
                                                      ImapMessageFlags.Flagged |
                                                      ImapMessageFlags.IsRead |
                                                      ImapMessageFlags.Recent;

                    // Remove the flags from each message
                    foreach (ImapMessageInfo info in messages)
                    {
                        // Remove flags using the message's sequence number
                        client.RemoveMessageFlags(new List<int> { info.SequenceNumber }, flagsToRemove);
                    }

                    Console.WriteLine("All flag attributes have been cleared from the messages.");
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
