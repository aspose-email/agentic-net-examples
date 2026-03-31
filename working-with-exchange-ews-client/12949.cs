using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailFolderMoveSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – real credentials should be provided by the user.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against executing real network calls with placeholder data.
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping actual server call.");
                    return;
                }

                // Create and connect the IMAP client.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Define source folder and the new parent folder.
                        string sourceFolderName = "SourceFolder";
                        string newParentFolderUri = "INBOX";

                        // Attempt to move the folder.
                        client.MoveFolder(newParentFolderUri, sourceFolderName);
                        Console.WriteLine($"Folder '{sourceFolderName}' successfully moved under '{newParentFolderUri}'.");
                    }
                    catch (Exception moveEx)
                    {
                        // Handle errors such as insufficient permissions or non‑existent folders.
                        Console.Error.WriteLine($"Failed to move folder: {moveEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
