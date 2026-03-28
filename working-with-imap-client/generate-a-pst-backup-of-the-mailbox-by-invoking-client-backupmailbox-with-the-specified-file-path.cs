using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection settings (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Path for the PST backup file
            string backupFilePath = "mailBackup.pst";

            // Ensure the directory for the backup file exists
            string backupDir = Path.GetDirectoryName(backupFilePath);
            if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                // Perform the backup of all folders to the specified PST file
                // ListFolders() returns the collection of folders to be backed up
                client.Backup(client.ListFolders(), backupFilePath, new BackupSettings());
                Console.WriteLine($"Backup completed successfully: {backupFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
