using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace ImapBackupSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IMAP server connection settings (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                // Path for the backup file
                string backupFilePath = "imap_backup.pst";

                // Ensure the directory for the backup file exists
                try
                {
                    string? directory = Path.GetDirectoryName(backupFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File system error: {ioEx.Message}");
                    return;
                }

                // Connect to the IMAP server and perform backup
                try
                {
                    using (ImapClient client = new ImapClient(host, port, username, password, security))
                    {
                        // Retrieve all folders from the mailbox
                        ImapFolderInfoCollection folders = client.ListFolders();

                        // Backup the selected folders to the specified PST file
                        BackupSettings backupSettings = new BackupSettings();
                        client.Backup(folders, backupFilePath, backupSettings);

                        Console.WriteLine($"Backup completed successfully. File saved to: {backupFilePath}");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
            catch (Exception topEx)
            {
                Console.Error.WriteLine($"Fatal error: {topEx.Message}");
            }
        }
    }
}
