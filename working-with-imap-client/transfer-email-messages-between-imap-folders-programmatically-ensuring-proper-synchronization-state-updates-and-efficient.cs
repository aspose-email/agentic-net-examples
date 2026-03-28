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
            // IMAP server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Source and destination folder names
            string sourceFolder = "INBOX";
            string destinationFolder = "Processed";

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                try
                {
                    // Validate credentials
                    client.ValidateCredentials();

                    // Ensure the destination folder exists
                    if (!client.ExistFolder(destinationFolder))
                    {
                        client.CreateFolder(destinationFolder);
                    }

                    // Select the source folder
                    client.SelectFolder(sourceFolder);

                    // Retrieve messages from the source folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    // Move each message to the destination folder
                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        client.MoveMessage(messageInfo.UniqueId, destinationFolder);
                    }

                    Console.WriteLine($"Moved {messages.Count} messages from '{sourceFolder}' to '{destinationFolder}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
