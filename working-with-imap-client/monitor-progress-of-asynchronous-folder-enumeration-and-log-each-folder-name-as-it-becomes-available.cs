using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailFolderMonitoring
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder credentials detection – skip real network calls in CI
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";

                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                    return;
                }

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Asynchronously retrieve the list of folders
                        ImapFolderInfoCollection folderCollection = await client.ListFoldersAsync();

                        // Log each folder name as it becomes available in the collection
                        foreach (ImapFolderInfo folderInfo in folderCollection)
                        {
                            Console.WriteLine($"Folder: {folderInfo.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during folder enumeration: {ex.Message}");
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
}
