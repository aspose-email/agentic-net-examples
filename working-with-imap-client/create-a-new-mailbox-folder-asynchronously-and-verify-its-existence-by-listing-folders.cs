using Aspose.Email;
using System;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials – skip execution if they are not real.
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";
                string newFolderName = "SampleFolder";

                if (string.IsNullOrWhiteSpace(host) ||
                    host.Contains("example", StringComparison.OrdinalIgnoreCase) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client.
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Create a new folder asynchronously.
                    try
                    {
                        await client.CreateFolderAsync(newFolderName);
                        Console.WriteLine($"Folder '{newFolderName}' created.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create folder: {ex.Message}");
                        return;
                    }

                    // Verify the folder exists.
                    try
                    {
                        bool exists = await client.ExistFolderAsync(newFolderName);
                        Console.WriteLine($"Folder existence check: {(exists ? "Exists" : "Does not exist")}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to verify folder existence: {ex.Message}");
                        return;
                    }

                    // List all folders to confirm.
                    try
                    {
                        var folders = await client.ListFoldersAsync();
                        Console.WriteLine("Mailbox folders:");
                        foreach (var folder in folders)
                        {
                            Console.WriteLine($"- {folder.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list folders: {ex.Message}");
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
