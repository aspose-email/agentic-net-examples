using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailRecovery
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths and server credentials (replace with real values or keep placeholders)
                string pstFilePath = "backup.pst";
                string imapHost = "imap.example.com";
                int imapPort = 993;
                string imapUsername = "user@example.com";
                string imapPassword = "password";

                // Ensure the PST backup file exists; create a minimal placeholder if missing
                if (!File.Exists(pstFilePath))
                {
                    try
                    {
                        using (PersonalStorage placeholderPst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                        {
                            // Create a default folder (Inbox) to keep hierarchy valid
                            placeholderPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                        }
                        Console.WriteLine($"Placeholder PST created at '{pstFilePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                        return;
                    }
                    // No data to restore, exit gracefully
                    return;
                }

                // Load the PST backup
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Configure restore settings to preserve folder hierarchy and metadata
                    RestoreSettings restoreSettings = new RestoreSettings
                    {
                        Recursive = true,                     // Restore subfolders recursively
                        RemoveNonexistentFolders = false,     // Keep existing folders on the server
                        RemoveNonexistentItems = false        // Keep existing items on the server
                    };

                    // Initialize the IMAP client (no explicit Connect method required)
                    using (ImapClient client = new ImapClient(imapHost, imapPort, imapUsername, imapPassword, SecurityOptions.Auto))
                    {
                        try
                        {
                            // Perform the restore operation
                            client.Restore(pst, restoreSettings);
                            Console.WriteLine("Mailbox data restored successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error during restore: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
