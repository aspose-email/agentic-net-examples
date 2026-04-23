using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Configuration (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP configuration detected. Skipping server synchronization.");
                return;
            }

            // Define the local folder hierarchy that should exist on the server
            List<string> localFolders = new List<string>
            {
                "Inbox",
                "Inbox/Projects",
                "Inbox/Projects/2024",
                "Inbox/Archives"
            };

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Validate credentials by selecting the INBOX (lightweight operation)
                try
                {
                    await client.SelectFolderAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to authenticate or select INBOX: {ex.Message}");
                    return;
                }

                // Retrieve the full list of existing server folders
                ImapFolderInfoCollection serverFolders;
                try
                {
                    serverFolders = await client.ListFoldersAsync(loadFullInfo: true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list server folders: {ex.Message}");
                    return;
                }

                // Helper to check if a folder exists on the server
                bool ServerFolderExists(string folderName)
                {
                    foreach (ImapFolderInfo info in serverFolders)
                    {
                        if (string.Equals(info.Name, folderName, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                    return false;
                }

                // Ensure each local folder exists on the server, creating missing ones
                foreach (string folderPath in localFolders)
                {
                    // Split the path to create intermediate folders if necessary
                    string[] parts = folderPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string cumulativePath = string.Empty;

                    foreach (string part in parts)
                    {
                        cumulativePath = string.IsNullOrEmpty(cumulativePath) ? part : $"{cumulativePath}/{part}";

                        if (!ServerFolderExists(cumulativePath))
                        {
                            try
                            {
                                await client.CreateFolderAsync(cumulativePath);
                                Console.WriteLine($"Created folder: {cumulativePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to create folder '{cumulativePath}': {ex.Message}");
                                // Continue with next folders even if one fails
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
