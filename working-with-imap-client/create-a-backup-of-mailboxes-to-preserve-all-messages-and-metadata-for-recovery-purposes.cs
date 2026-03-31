using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapBackupExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";
                string backupFilePath = "imap_backup.pst";

                // Guard: skip execution when placeholder credentials are used
                if (host.Contains("example") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                    return;
                }

                // Ensure the output directory exists
                string backupDirectory = Path.GetDirectoryName(backupFilePath);
                if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate connection and credentials
                        client.ValidateCredentials();
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP connection failed: {imapEx.Message}");
                        return;
                    }

                    // Retrieve all folders to be backed up
                    ImapFolderInfoCollection folders = client.ListFolders();

                    // Backup settings (default configuration)
                    BackupSettings backupSettings = new BackupSettings();

                    // Perform the backup to the specified file
                    client.Backup(folders, backupFilePath, backupSettings);
                    Console.WriteLine($"Backup completed successfully. File saved at: {backupFilePath}");
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
