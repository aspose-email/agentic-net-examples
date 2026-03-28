using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize IMAP client with connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;

                    // Rename the folder
                    string oldFolderName = "OldFolder";
                    string newFolderName = "NewFolder";
                    client.RenameFolder(oldFolderName, newFolderName);
                    Console.WriteLine($"Folder renamed from '{oldFolderName}' to '{newFolderName}'.");
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
