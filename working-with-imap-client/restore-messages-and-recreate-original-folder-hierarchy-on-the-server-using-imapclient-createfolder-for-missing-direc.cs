using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Input parameters (replace with real values or keep placeholders)
            string pstPath = "backup.pst";
            string imapHost = "imap.example.com";
            int imapPort = 993;
            string imapUsername = "username";
            string imapPassword = "password";

            // Guard file existence
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a minimal placeholder PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.Error.WriteLine($"Placeholder PST created at '{pstPath}'. No data to restore.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                }
                return;
            }

            // Load the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Create and connect the IMAP client
                try
                {
                    using (ImapClient client = new ImapClient(imapHost, imapPort, imapUsername, imapPassword))
                    {
                        // Recreate folder hierarchy on the server
                        RecreateFolderHierarchy(pst.RootFolder, client, string.Empty);

                        // Restore messages from PST to the server
                        client.Restore(pst, new RestoreSettings());
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively ensure each PST folder exists on the IMAP server
    private static void RecreateFolderHierarchy(FolderInfo pstFolder, ImapClient client, string parentPath)
    {
        // Get subfolders of the current PST folder
        FolderInfoCollection subFolders = pstFolder.GetSubFolders();

        foreach (FolderInfo subFolder in subFolders)
        {
            string folderPath = string.IsNullOrEmpty(parentPath)
                ? subFolder.DisplayName
                : $"{parentPath}/{subFolder.DisplayName}";

            // Check if the folder already exists on the server
            bool exists;
            try
            {
                exists = client.ExistFolder(folderPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to check existence of folder '{folderPath}': {ex.Message}");
                continue;
            }

            // Create the folder if it does not exist
            if (!exists)
            {
                try
                {
                    client.CreateFolder(folderPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create folder '{folderPath}': {ex.Message}");
                    continue;
                }
            }

            // Recurse into subfolders
            RecreateFolderHierarchy(subFolder, client, folderPath);
        }
    }
}
