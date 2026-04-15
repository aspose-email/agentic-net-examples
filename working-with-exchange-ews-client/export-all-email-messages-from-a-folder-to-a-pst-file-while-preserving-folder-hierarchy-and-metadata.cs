using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string folderUri = "Inbox"; // distinguished folder name or folder URI
            string pstPath = "Exported.pst";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure target directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Retrieve folder information
            ExchangeFolderInfo folderInfo;
            try
            {
                folderInfo = client.GetFolderInfo(folderUri);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to get folder info: {ex.Message}");
                return;
            }

            // Prepare folder collection for backup
            ExchangeFolderInfoCollection folders = new ExchangeFolderInfoCollection();
            folders.Add(folderInfo);

            // Perform backup to PST
            try
            {
                BackupOptions backupOptions = new BackupOptions();
                client.Backup(folders, pstPath, backupOptions);
                Console.WriteLine($"Backup completed successfully to '{pstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Backup failed: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
