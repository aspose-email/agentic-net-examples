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
        // Placeholder credentials – replace with real values or skip execution.
        string host = "imap.example.com";
        string username = "user@example.com";
        string password = "password";

        // Detect placeholder credentials and avoid network calls.
        if (host.Contains("example") || username.Contains("example") || password.Equals("password", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP backup operation.");
            return;
        }

        // Destination PST file.
        string backupPath = "backup.pst";

        // Ensure the directory for the backup file exists.
        try
        {
            string? directory = Path.GetDirectoryName(Path.GetFullPath(backupPath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare backup directory: {ex.Message}");
            return;
        }

        // Perform the backup.
        try
        {
            // Create and connect the IMAP client. The constructor establishes the connection.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Retrieve all folders to be backed up.
                ImapFolderInfoCollection folders = client.ListFolders();

                // Backup settings – default options.
                BackupSettings settings = new BackupSettings();

                // Execute the backup to the specified PST file.
                client.Backup(folders, backupPath, settings);
            }

            Console.WriteLine($"Mailbox backup completed successfully: {backupPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during mailbox backup: {ex.Message}");
        }
    }
}
