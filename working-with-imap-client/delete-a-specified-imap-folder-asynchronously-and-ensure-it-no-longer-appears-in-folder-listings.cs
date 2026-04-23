using Aspose.Email.Storage.Pst;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip execution in CI environments.
            string host = "imap.example.com";
            int port = 993;
            bool useSsl = true;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, useSsl))
            {
                try
                {
                    // Folder to delete.
                    string folderToDelete = "TestFolder";

                    // List folders before deletion.
                    ImapFolderInfoCollection foldersBefore = await client.ListFoldersAsync();
                    Console.WriteLine("Folders before deletion:");
                    foreach (ImapFolderInfo info in foldersBefore)
                    {
                        Console.WriteLine($"- {info.Name}");
                    }

                    // Delete the specified folder asynchronously.
                    await client.DeleteFolderAsync(folderToDelete);

                    // Verify the folder no longer exists.
                    bool exists = await client.ExistFolderAsync(folderToDelete);
                    Console.WriteLine($"Folder \"{folderToDelete}\" exists after deletion: {exists}");

                    // List folders after deletion.
                    ImapFolderInfoCollection foldersAfter = await client.ListFoldersAsync();
                    Console.WriteLine("Folders after deletion:");
                    foreach (ImapFolderInfo info in foldersAfter)
                    {
                        Console.WriteLine($"- {info.Name}");
                    }
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
