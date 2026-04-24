using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration placeholders
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string backupFolderPath = @"\\networkshare\emailbackups";
            string backupFileName = "imap_backup.pst";
            string backupPath = Path.Combine(backupFolderPath, backupFileName);

            // Guard against placeholder credentials/host
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                return;
            }

            // Ensure the UNC directory exists
            try
            {
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to verify or create backup directory: {dirEx.Message}");
                return;
            }

            // Ensure we can write to the backup file path
            try
            {
                using (FileStream fs = new FileStream(backupPath, FileMode.Create, FileAccess.Write))
                {
                    // Just create an empty file to test write permission, then close.
                }
                // Delete the empty test file; actual backup will recreate it.
                File.Delete(backupPath);
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Cannot write to backup path: {fileEx.Message}");
                return;
            }

            // Connect to IMAP server and perform backup
            try
            {
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    client.Username = username;
                    client.Password = password;

                    // Validate connection by selecting INBOX
                    client.SelectFolder("INBOX");

                    // Retrieve all folders to backup
                    ImapFolderInfoCollection folders = client.ListFolders();

                    // Perform backup to the specified UNC path
                    BackupSettings backupSettings = new BackupSettings();
                    client.Backup(folders, backupPath, backupSettings);
                }
            }
            catch (Exception imapEx)
            {
                Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                return;
            }

            Console.WriteLine("Backup completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
