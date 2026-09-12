using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Path for the backup file
            string backupFilePath = "backup.pst";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the directory for the backup file exists
            string backupDir = Path.GetDirectoryName(backupFilePath);
            if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            // Create and configure the ImapClient (client variable name preserved)
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.Username = username;
                imapClient.Password = password;
                imapClient.SecurityOptions = SecurityOptions.SSLImplicit;

                // Retrieve all folders from the mailbox
                ImapFolderInfoCollection folders = imapClient.ListFolders();

                // Backup options (default settings)
                BackupSettings backupOptions = new BackupSettings();

                // Perform the backup of the selected folders
                imapClient.Backup(folders, backupFilePath, backupOptions);
                Console.WriteLine("Mailbox backup completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
