using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // IMAP server connection settings (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to connect to IMAP server: " + ex.Message);
                    return;
                }

                // List existing folders
                Console.WriteLine("Existing folders:");
                ImapFolderInfoCollection existingFolders = client.ListFolders();
                foreach (ImapFolderInfo folderInfo in existingFolders)
                {
                    Console.WriteLine("- " + folderInfo.Name);
                }

                // Create a new folder named "TestFolder"
                string newFolderName = "TestFolder";
                client.CreateFolder(newFolderName);
                Console.WriteLine($"Folder \"{newFolderName}\" created.");

                // Rename the folder to "RenamedFolder"
                string renamedFolderName = "RenamedFolder";
                client.RenameFolder(newFolderName, renamedFolderName);
                Console.WriteLine($"Folder \"{newFolderName}\" renamed to \"{renamedFolderName}\".");

                // Delete the renamed folder
                client.DeleteFolder(renamedFolderName);
                Console.WriteLine($"Folder \"{renamedFolderName}\" deleted.");

                // List folders after modifications
                Console.WriteLine("Folders after operations:");
                ImapFolderInfoCollection finalFolders = client.ListFolders();
                foreach (ImapFolderInfo folderInfo in finalFolders)
                {
                    Console.WriteLine("- " + folderInfo.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
