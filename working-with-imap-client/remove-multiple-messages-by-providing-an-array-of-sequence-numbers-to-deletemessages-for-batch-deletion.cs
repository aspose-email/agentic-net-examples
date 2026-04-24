using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            bool useSsl = true;
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Sequence numbers of messages to delete
            int[] messagesToDelete = new int[] { 1, 2, 3 };

            using (ImapClient client = new ImapClient(host, port, username, password, useSsl))
            {
                try
                {
                    // Retrieve information about all messages in the selected folder (INBOX by default)
                    ImapMessageInfoCollection allMessages = client.ListMessages();

                    // Filter the messages that match the requested sequence numbers
                    List<ImapMessageInfo> targetMessages = allMessages
                        .Where(info => messagesToDelete.Contains(info.SequenceNumber))
                        .ToList();

                    if (targetMessages.Count == 0)
                    {
                        Console.WriteLine("No messages found with the specified sequence numbers.");
                        return;
                    }

                    // Delete the selected messages in a batch operation
                    client.DeleteMessages(targetMessages);
                    Console.WriteLine($"Deleted {targetMessages.Count} message(s) successfully.");
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
