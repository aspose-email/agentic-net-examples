using Aspose.Email.Storage.Pst;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main()
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected – skipping network operations.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                // Wrap client connection in try/catch (client connection safety)
                try
                {
                    // No explicit Connect call – operations will establish connection as needed
                    // Rename an existing folder asynchronously
                    string oldFolderName = "OldFolder";
                    string newFolderName = "NewFolder";

                    await client.RenameFolderAsync(oldFolderName, newFolderName);

                    // Confirm the rename by fetching the updated list of folders
                    ImapFolderInfoCollection folders = await client.ListFoldersAsync();

                    Console.WriteLine("Folders after rename:");
                    foreach (ImapFolderInfo folder in folders)
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
                }
                catch (Exception ex)
                {
                    // Client‑level error handling
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Global error handling
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
