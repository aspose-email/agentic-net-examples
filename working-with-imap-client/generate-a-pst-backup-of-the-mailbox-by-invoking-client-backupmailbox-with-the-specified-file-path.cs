using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IMAP server connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Path to the backup PST file
                string backupFilePath = "backup/mailBackup.pst";

                // Ensure the directory for the backup file exists
                string backupDirectory = Path.GetDirectoryName(backupFilePath);
                if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                // Create and use the IMAP client
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Retrieve all folders from the mailbox
                    ImapFolderInfoCollection folders = imapClient.ListFolders();

                    // Use default backup settings
                    BackupSettings backupSettings = BackupSettings.Default;

                    // Perform the backup to the specified PST file
                    imapClient.Backup(folders, backupFilePath, backupSettings);

                    Console.WriteLine("Mailbox backup completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}