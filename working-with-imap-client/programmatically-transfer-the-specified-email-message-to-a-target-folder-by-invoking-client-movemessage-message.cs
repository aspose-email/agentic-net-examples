using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string sourceFolder = "INBOX";
            string destinationFolder = "Processed";

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Ensure the source folder exists
                    if (!client.ExistFolder(sourceFolder))
                    {
                        Console.Error.WriteLine($"Source folder \"{sourceFolder}\" does not exist.");
                        return;
                    }

                    // Ensure the destination folder exists; create it if missing
                    if (!client.ExistFolder(destinationFolder))
                    {
                        client.CreateFolder(destinationFolder);
                    }

                    // Select the source folder
                    client.SelectFolder(sourceFolder);

                    // Retrieve the list of messages in the source folder
                    ImapMessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages to move.");
                        return;
                    }

                    // Take the first message's unique identifier
                    ImapMessageInfo firstMessage = messages[0];
                    string uniqueId = firstMessage.UniqueId;

                    // Move the message to the destination folder
                    string newUid = client.MoveMessage(uniqueId, destinationFolder);
                    Console.WriteLine($"Message UID {uniqueId} moved to \"{destinationFolder}\". New UID: {newUid}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
