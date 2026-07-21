using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Aspose.Email example – bulk delete messages via IMAP
class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the ImapClient
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.Username = username;
                imapClient.Password = password;
                imapClient.SecurityOptions = SecurityOptions.SSLImplicit;

                try
                {
                    // Retrieve all messages in the mailbox
                    ImapMessageInfoCollection allMessages = imapClient.ListMessages();

                    // Choose a subset to delete (e.g., first 10 messages)
                    List<ImapMessageInfo> messagesToDelete = allMessages.Take(10).ToList();

                    if (messagesToDelete.Count > 0)
                    {
                        // Bulk delete the selected messages
                        imapClient.DeleteMessages(messagesToDelete);

                        // Permanently remove the marked messages from the server
                        Console.WriteLine($"{messagesToDelete.Count} messages have been deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages matched the deletion criteria.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Deletion error: {ex.Message}");
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
