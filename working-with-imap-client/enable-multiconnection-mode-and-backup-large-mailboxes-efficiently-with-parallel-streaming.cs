using Aspose.Email.Storage.Pst;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using System;
using System.IO;

namespace AsposeEmailImapBackup
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values when running against an actual server
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                    return;
                }

                // Path for the PST backup file
                string backupFilePath = Path.Combine("Backup", "mailbox_backup.pst");

                // Ensure the backup directory exists
                try
                {
                    string backupDirectory = Path.GetDirectoryName(backupFilePath);
                    if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
                    {
                        Directory.CreateDirectory(backupDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare backup directory: {dirEx.Message}");
                    return;
                }

                // Connect to the IMAP server and perform the backup
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // If the used Aspose.Email version supports multi‑connection, enable it here.
                        // Example (uncomment when available):
                        // client.MultiConnectionMode = MultiConnectionMode.Enable;
                        // client.ConnectionsQuantity = 4;

                        // List all folders in the mailbox
                        ImapFolderInfoCollection folders = client.ListFolders();

                        // Backup the mailbox to a PST file
                        client.Backup(folders, backupFilePath, BackupSettings.Default);

                        Console.WriteLine("Mailbox backup completed successfully.");
                    }
                    catch (Exception imapEx)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
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
