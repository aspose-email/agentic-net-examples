using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize IMAP client with connection parameters
            using (ImapClient client = new ImapClient("imap.example.com", 993, "user@example.com", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Connect to the server

                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve a list of messages from the folder
                    List<ImapMessageInfo> allMessages = client.ListMessages();

                    // Choose messages to delete (e.g., first 5 messages)
                    List<ImapMessageInfo> messagesToDelete = allMessages.GetRange(0, Math.Min(5, allMessages.Count));

                    // Perform bulk deletion and commit immediately
                    client.DeleteMessages(messagesToDelete, commitNow: true);

                    Console.WriteLine($"{messagesToDelete.Count} messages have been deleted.");
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
