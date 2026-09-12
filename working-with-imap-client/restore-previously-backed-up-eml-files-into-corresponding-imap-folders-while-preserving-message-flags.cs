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
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrEmpty(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP restore.");
                return;
            }

            // Path to the PST file that contains the backed‑up .eml messages
            string backupPath = "backup.pst";

            // Ensure the backup PST exists; create a minimal placeholder if missing
            if (!File.Exists(backupPath))
            {
                try
                {
                    PersonalStorage.Create(backupPath, FileFormatVersion.Unicode);
                    Console.Error.WriteLine($"Backup file not found. Created empty placeholder PST at '{backupPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                }
                return;
            }

            // Load the personal storage (PST) containing the messages
            PersonalStorage pst;
            try
            {
                pst = PersonalStorage.FromFile(backupPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
                return;
            }

            // Perform the restore using ImapClient
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    client.Restore(pst, new RestoreSettings());
                    Console.WriteLine("Restore completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP restore failed: {ex.Message}");
                }
            }

            // Dispose the PST storage
            pst.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
