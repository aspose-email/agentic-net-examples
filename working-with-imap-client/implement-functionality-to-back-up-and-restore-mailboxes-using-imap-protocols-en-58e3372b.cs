using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

namespace ImapBackupRestoreExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // IMAP server connection settings (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Path for the backup file
                string backupFilePath = Path.Combine(Environment.CurrentDirectory, "imap_backup.pst");

                // Ensure the directory for the backup file exists
                string backupDirectory = Path.GetDirectoryName(backupFilePath);
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                // Create and use the IMAP client
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // List all folders to be backed up
                        ImapFolderInfoCollection folders = imapClient.ListFolders();

                        // Perform backup to the specified PST file
                        imapClient.Backup(folders, backupFilePath, new Aspose.Email.Clients.Imap.BackupSettings());

                        Console.WriteLine("Backup completed successfully to: " + backupFilePath);
                    }
                    catch (Exception clientEx)
                    {
                        Console.Error.WriteLine("IMAP operation failed: " + clientEx.Message);
                        return;
                    }

                    // Verify that the backup file was created before attempting restore
                    if (!File.Exists(backupFilePath))
                    {
                        Console.Error.WriteLine("Backup file not found: " + backupFilePath);
                        return;
                    }

                    // Open the backup PST file
                    using (PersonalStorage pst = PersonalStorage.FromFile(backupFilePath))
                    {
                        // Configure restore settings (default settings used here)
                        Aspose.Email.Clients.Imap.RestoreSettings restoreSettings = new Aspose.Email.Clients.Imap.RestoreSettings();

                        try
                        {
                            // Perform restore from the PST file
                            imapClient.Restore(pst, restoreSettings);
                            Console.WriteLine("Restore completed successfully from: " + backupFilePath);
                        }
                        catch (Exception restoreEx)
                        {
                            Console.Error.WriteLine("Restore operation failed: " + restoreEx.Message);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}