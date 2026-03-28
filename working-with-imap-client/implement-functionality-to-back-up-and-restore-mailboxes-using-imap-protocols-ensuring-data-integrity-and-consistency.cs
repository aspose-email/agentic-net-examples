using Aspose.Email.Clients;
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
            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Path for the backup file
            string backupFilePath = "imap_backup.pst";

            // Ensure the backup directory exists
            try
            {
                string backupDir = Path.GetDirectoryName(backupFilePath);
                if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare backup directory: {ex.Message}");
                return;
            }

            // Create and use the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // ------------------- Backup -------------------
                    try
                    {
                        ImapFolderInfoCollection folders = client.ListFolders();
                        using (FileStream backupStream = new FileStream(backupFilePath, FileMode.Create, FileAccess.Write))
                        {
                            BackupSettings backupSettings = new BackupSettings();
                            client.Backup(folders, backupStream, backupSettings);
                            Console.WriteLine("Backup completed successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Backup failed: {ex.Message}");
                        return;
                    }

                    // Verify backup file exists before restore
                    if (!File.Exists(backupFilePath))
                    {
                        Console.Error.WriteLine("Backup file not found for restore operation.");
                        return;
                    }

                    // ------------------- Restore -------------------
                    try
                    {
                        using (PersonalStorage pst = PersonalStorage.FromFile(backupFilePath))
                        {
                            RestoreSettings restoreSettings = new RestoreSettings();
                            client.Restore(pst, restoreSettings);
                            Console.WriteLine("Restore completed successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Restore failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
