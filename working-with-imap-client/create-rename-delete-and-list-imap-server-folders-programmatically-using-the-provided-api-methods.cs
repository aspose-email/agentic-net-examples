using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapFolderManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // Author note: Sample demonstrates creating, renaming, deleting, and listing IMAP folders.
            try
            {
                // Define connection parameters
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

                // Initialize the IMAP client (IDisposable)
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Create a new folder named "DemoFolder"
                        imapClient.CreateFolder("DemoFolder");
                        Console.WriteLine("Folder 'DemoFolder' created.");

                        // Rename the folder to "RenamedDemoFolder"
                        imapClient.RenameFolder("DemoFolder", "RenamedDemoFolder");
                        Console.WriteLine("Folder renamed to 'RenamedDemoFolder'.");

                        // List all folders in the mailbox
                        ImapFolderInfoCollection folders = imapClient.ListFolders();
                        Console.WriteLine("Current folders:");
                        foreach (ImapFolderInfo folder in folders)
                        {
                            Console.WriteLine($"- {folder.Name}");
                        }

                        // Delete the renamed folder
                        imapClient.DeleteFolder("RenamedDemoFolder");
                        Console.WriteLine("Folder 'RenamedDemoFolder' deleted.");

                        // List folders again to confirm deletion
                        ImapFolderInfoCollection finalFolders = imapClient.ListFolders();
                        Console.WriteLine("Folders after deletion:");
                        foreach (ImapFolderInfo folder in finalFolders)
                        {
                            Console.WriteLine($"- {folder.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                        // Optionally, handle specific errors or cleanup here
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
