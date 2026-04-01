using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Tools.Search;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                string destinationFolder = "Processed";

                // Guard against executing real network calls with placeholder data
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the source folder (INBOX)
                        client.SelectFolder("INBOX");

                        // Retrieve the list of messages in the selected folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found to move.");
                            return;
                        }

                        // Take the first message's unique identifier
                        ImapMessageInfo firstMessage = messages[0];
                        string uniqueId = firstMessage.UniqueId;

                        // Move the message to the destination folder
                        string newUniqueId = client.MoveMessage(uniqueId, destinationFolder);

                        Console.WriteLine($"Message moved successfully. New UID: {newUniqueId}");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
