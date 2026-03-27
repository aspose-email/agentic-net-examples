using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file that contains the backup.
            string pstFilePath = "backup.pst";

            // Verify that the PST file exists before attempting to open it.
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Open the PST file inside a using block to ensure proper disposal.
            using (PersonalStorage personalStorage = PersonalStorage.FromFile(pstFilePath))
            {
                // Create and configure the IMAP client.
                using (ImapClient imapClient = new ImapClient("imap.example.com", 993, Aspose.Email.Clients.SecurityOptions.Auto))
                {
                    // Set authentication credentials.
                    imapClient.Username = "user@example.com";
                    imapClient.Password = "password";

                    // Optional: verify connection by issuing a NOOP command.
                    try
                    {
                        imapClient.Noop();
                    }
                    catch (Exception connectionEx)
                    {
                        Console.Error.WriteLine($"Failed to connect to IMAP server: {connectionEx.Message}");
                        return;
                    }

                    // Prepare restore settings (default settings are sufficient for a basic restore).
                    RestoreSettings restoreSettings = new RestoreSettings();

                    // Perform the restore operation.
                    try
                    {
                        imapClient.Restore(personalStorage, restoreSettings);
                        Console.WriteLine("Mailbox restored successfully from the PST backup.");
                    }
                    catch (Exception restoreEx)
                    {
                        Console.Error.WriteLine($"Error during restore: {restoreEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}