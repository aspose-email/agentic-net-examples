using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Ensure the backup directory exists
            string backupPath = "backup.pst";
            string backupDirectory = Path.GetDirectoryName(backupPath);
            if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
            {
                try
                {
                    Directory.CreateDirectory(backupDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create backup directory: {dirEx.Message}");
                    return;
                }
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;

                    // Select the INBOX folder before any operation
                    client.SelectFolder("INBOX");

                    // Prepare folder collection for backup (INBOX only)
                    ImapFolderInfoCollection folders = new ImapFolderInfoCollection();
                    ImapFolderInfo inboxInfo = client.GetFolderInfo("INBOX");
                    folders.Add(inboxInfo);

                    // Backup settings (default)
                    BackupSettings backupSettings = new BackupSettings();

                    // Perform backup to a file stream
                    using (FileStream backupStream = new FileStream(backupPath, FileMode.Create, FileAccess.Write))
                    {
                        try
                        {
                            client.Backup(folders, backupStream, backupSettings);
                            Console.WriteLine($"Backup of INBOX completed successfully to '{backupPath}'.");
                        }
                        catch (Exception backupEx)
                        {
                            Console.Error.WriteLine($"Backup failed: {backupEx.Message}");
                            return;
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
