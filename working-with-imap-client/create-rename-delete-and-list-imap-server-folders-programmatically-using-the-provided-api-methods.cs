using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapFolderManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – skip actual network calls in CI environments
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                if (host.Contains("example") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP folder operations.");
                    return;
                }

                // Initialize the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, security))
                {
                    try
                    {
                        // List existing folders
                        Console.WriteLine("Existing folders:");
                        ImapFolderInfoCollection folders = client.ListFolders();
                        foreach (ImapFolderInfo folder in folders)
                        {
                            Console.WriteLine("- " + folder.Name);
                        }

                        // Create a new folder
                        string newFolder = "MyNewFolder";
                        client.CreateFolder(newFolder);
                        Console.WriteLine($"Folder '{newFolder}' created.");

                        // Rename the newly created folder
                        string renamedFolder = "MyRenamedFolder";
                        client.RenameFolder(newFolder, renamedFolder);
                        Console.WriteLine($"Folder '{newFolder}' renamed to '{renamedFolder}'.");

                        // Delete the renamed folder
                        client.DeleteFolder(renamedFolder);
                        Console.WriteLine($"Folder '{renamedFolder}' deleted.");

                        // List folders again to confirm changes
                        Console.WriteLine("Folders after operations:");
                        ImapFolderInfoCollection updatedFolders = client.ListFolders();
                        foreach (ImapFolderInfo folder in updatedFolders)
                        {
                            Console.WriteLine("- " + folder.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("IMAP operation failed: " + ex.Message);
                        // No rethrow – graceful exit
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
