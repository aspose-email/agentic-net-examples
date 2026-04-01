using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping actual server connection.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    IEnumerable<ImapMessageInfo> allMessages = client.ListMessages();

                    // Choose a subset of messages to delete (e.g., first 5)
                    List<ImapMessageInfo> messagesToDelete = allMessages.Take(5).ToList();

                    if (messagesToDelete.Count == 0)
                    {
                        Console.WriteLine("No messages found to delete.");
                        return;
                    }

                    // Bulk delete the selected messages and commit immediately
                    client.DeleteMessages(messagesToDelete);

                    Console.WriteLine($"{messagesToDelete.Count} messages have been deleted.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
