using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993; // default IMAP SSL port
            string username = "username";
            string password = "password";
            string backupFilePath = "backup.pst";

            // Skip execution if placeholders are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                return;
            }

            // Ensure the backup directory exists
            try
            {
                string backupDirectory = Path.GetDirectoryName(backupFilePath);
                if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare backup directory: {ioEx.Message}");
                return;
            }

            // Create and use the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password))
                {
                    // Retrieve all folders (or filter for a specific folder if needed)
                    ImapFolderInfoCollection folders = client.ListFolders();

                    // Prepare backup settings (default options)
                    BackupSettings backupSettings = new BackupSettings();

                    // Perform the backup to the PST file
                    client.Backup(folders, backupFilePath, backupSettings);

                    Console.WriteLine($"Backup completed successfully to '{backupFilePath}'.");
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
