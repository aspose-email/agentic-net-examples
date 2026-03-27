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
            // Path where the backup will be stored
            string backupPath = "mailbox_backup.zip";

            // Ensure the directory for the backup file exists
            string backupDirectory = Path.GetDirectoryName(backupPath);
            if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            // Initialize the IMAP client (replace with real server and credentials)
            using (ImapClient client = new ImapClient("imap.example.com", 993, "user@example.com", "password", SecurityOptions.SSLImplicit))
            {
                // Simple connectivity check
                try
                {
                    client.Noop();
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"IMAP connection failed: {connEx.Message}");
                    return;
                }

                // Retrieve all folders from the mailbox
                ImapFolderInfoCollection folders = client.ListFolders();

                // Use default backup settings
                BackupSettings backupSettings = new BackupSettings();

                // Perform the backup synchronously (await the async task)
                client.BackupAsync(folders, backupPath, backupSettings).GetAwaiter().GetResult();

                Console.WriteLine("Mailbox backup completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}