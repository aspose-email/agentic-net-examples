using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values for actual use
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string backupFilePath = "mailbox_backup.pst";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string backupDir = Path.GetDirectoryName(backupFilePath);
                if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File system error: {ioEx.Message}");
                return;
            }

            // Connect to the IMAP server and perform the backup
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve all folders from the mailbox
                    ImapFolderInfoCollection folders = client.ListFolders();

                    // Configure backup settings (default settings used here)
                    BackupSettings backupSettings = new BackupSettings();

                    // Execute the backup to the specified PST file
                    client.Backup(folders, backupFilePath, backupSettings);

                    Console.WriteLine($"Backup completed successfully: {backupFilePath}");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during backup: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
