using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace DeleteBatchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize IMAP client with server details
                using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
                {
                    // Select the folder containing messages
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    List<ImapMessageInfo> allMessages = client.ListMessages();

                    // Determine how many messages to delete (e.g., first 5 or all if fewer)
                    int deleteCount = Math.Min(5, allMessages.Count);
                    List<ImapMessageInfo> messagesToDelete = allMessages.GetRange(0, deleteCount);

                    // Delete the selected batch and commit the deletions immediately
                    client.DeleteMessages(messagesToDelete, true);
                }
            }
            catch (Exception ex)
            {
                // Output any errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
