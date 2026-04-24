using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Server connection details (placeholders)
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder server/credentials detected. Skipping backup.");
                return;
            }

            // Path for the backup PST file
            string backupFilePath = "Backup\\imap_backup.pst";

            // Ensure the backup directory exists
            string backupDirectory = Path.GetDirectoryName(backupFilePath);
            if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            // Create and use the IMAP client
            using (ImapClient imapClient = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve the full folder hierarchy from the server
                    ImapFolderInfoCollection allFolders = imapClient.ListFolders();

                    // Backup settings (default)
                    BackupSettings backupSettings = new BackupSettings();

                    // Perform the backup preserving folder structure
                    imapClient.Backup(allFolders, backupFilePath, backupSettings);

                    Console.WriteLine($"Backup completed successfully: {backupFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during backup operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
