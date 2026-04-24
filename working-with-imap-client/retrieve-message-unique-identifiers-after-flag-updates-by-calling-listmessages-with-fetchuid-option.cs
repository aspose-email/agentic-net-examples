using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network call if they are not replaced.
            string host = "imap.example.com";
            int port = 993;
            string username = "username@example.com";
            string password = "password";

            bool isPlaceholder = host.Contains("example.com") ||
                                  username.Contains("username") ||
                                  password == "password";

            if (isPlaceholder)
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages (includes UniqueId).
                    IEnumerable<ImapMessageInfo> messages = client.ListMessages();

                    // If there are no messages, exit gracefully.
                    if (!messages.Any())
                    {
                        Console.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Take the first message.
                    ImapMessageInfo firstMessage = messages.First();

                    // Update the flag (mark as read) using the correct flag property.
                    client.AddMessageFlags(firstMessage.UniqueId, ImapMessageFlags.IsRead);

                    // Retrieve the messages again to get updated flags and unique IDs.
                    IEnumerable<ImapMessageInfo> updatedMessages = client.ListMessages();

                    // Output unique identifiers and their flags.
                    foreach (ImapMessageInfo info in updatedMessages)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Flags: {info.Flags}");
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
