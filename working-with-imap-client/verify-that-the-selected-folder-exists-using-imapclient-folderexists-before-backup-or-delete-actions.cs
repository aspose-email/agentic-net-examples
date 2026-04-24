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
            // Placeholder check – skip real network calls when using example credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Name of the folder to work with
                    string folderName = "TargetFolder";

                    // Verify that the folder exists before any operation
                    ImapFolderInfo folderInfo;
                    bool folderExists = client.ExistFolder(folderName, out folderInfo);
                    if (!folderExists)
                    {
                        Console.Error.WriteLine($"Folder '{folderName}' does not exist.");
                        return;
                    }

                    // ---------- Backup the folder ----------
                    string backupPath = "backup.pst";

                    // Ensure the directory for the backup file exists
                    string backupDir = Path.GetDirectoryName(backupPath);
                    if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                    {
                        try
                        {
                            Directory.CreateDirectory(backupDir);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create directory '{backupDir}': {ex.Message}");
                            return;
                        }
                    }

                    try
                    {
                        // Prepare the collection containing the folder to backup
                        ImapFolderInfoCollection foldersToBackup = new ImapFolderInfoCollection();
                        foldersToBackup.Add(folderInfo);

                        // Backup settings (default)
                        BackupSettings backupSettings = new BackupSettings();

                        // Perform the backup
                        client.Backup(foldersToBackup, backupPath, backupSettings);
                        Console.WriteLine($"Folder '{folderName}' backed up to '{backupPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Backup failed: {ex.Message}");
                    }

                    // ---------- Delete the folder ----------
                    try
                    {
                        client.DeleteFolder(folderName);
                        Console.WriteLine($"Folder '{folderName}' deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Delete failed: {ex.Message}");
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
