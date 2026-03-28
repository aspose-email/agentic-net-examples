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
            // Define IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Path to the PST file that contains the backup
            string pstPath = "backup.pst";

            // Verify that the PST file exists before proceeding
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Load the PST file into a PersonalStorage object
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Configure restore settings
                RestoreSettings restoreSettings = new RestoreSettings();
                // Use desired restore options (e.g., recursive restore and removal of nonexistent items)
                restoreSettings.Options = RestoreOptions.Recursive | RestoreOptions.RemoveNonexistentItems;

                // Create and connect the IMAP client
                try
                {
                    using (ImapClient client = new ImapClient(host, port, username, password, security))
                    {
                        // Begin the restore operation
                        client.Restore(pst, restoreSettings);
                        Console.WriteLine("Mailbox restored successfully from PST.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP client error: {ex.Message}");
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
