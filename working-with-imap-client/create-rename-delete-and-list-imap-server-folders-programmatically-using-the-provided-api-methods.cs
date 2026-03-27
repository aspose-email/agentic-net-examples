using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapFolderManagementExample
{
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
                        // Optional: verify connection
                        client.Noop();
                    }
                    catch (Exception connectionEx)
                    {
                        Console.Error.WriteLine($"IMAP connection failed: {connectionEx.Message}");
                        return;
                    }

                    // 1. Create a new folder
                    string newFolderName = "TestFolder";
                    try
                    {
                        client.CreateFolder(newFolderName);
                        Console.WriteLine($"Folder '{newFolderName}' created.");
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Failed to create folder '{newFolderName}': {createEx.Message}");
                    }

                    // List folders after creation
                    ListAllFolders(client);

                    // 2. Rename the folder
                    string renamedFolderName = "RenamedFolder";
                    try
                    {
                        client.RenameFolder(newFolderName, renamedFolderName);
                        Console.WriteLine($"Folder '{newFolderName}' renamed to '{renamedFolderName}'.");
                    }
                    catch (Exception renameEx)
                    {
                        Console.Error.WriteLine($"Failed to rename folder '{newFolderName}': {renameEx.Message}");
                    }

                    // List folders after rename
                    ListAllFolders(client);

                    // 3. Delete the renamed folder
                    try
                    {
                        client.DeleteFolder(renamedFolderName);
                        Console.WriteLine($"Folder '{renamedFolderName}' deleted.");
                    }
                    catch (Exception deleteEx)
                    {
                        Console.Error.WriteLine($"Failed to delete folder '{renamedFolderName}': {deleteEx.Message}");
                    }

                    // List folders after deletion
                    ListAllFolders(client);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void ListAllFolders(ImapClient client)
        {
            try
            {
                ImapFolderInfoCollection folders = client.ListFolders();
                Console.WriteLine("Current IMAP folders:");
                foreach (ImapFolderInfo folder in folders)
                {
                    Console.WriteLine($"- {folder.Name}");
                }
                Console.WriteLine();
            }
            catch (Exception listEx)
            {
                Console.Error.WriteLine($"Failed to list folders: {listEx.Message}");
            }
        }
    }
}