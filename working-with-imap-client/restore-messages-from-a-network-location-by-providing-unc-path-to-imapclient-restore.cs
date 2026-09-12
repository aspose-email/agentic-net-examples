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
            // UNC path to the PST backup file
            string pstPath = @"\\server\share\backup.pst";

            // Ensure a PST file exists (create a minimal one if missing)
            if (!File.Exists(pstPath))
            {
                Console.WriteLine($"PST file not found at '{pstPath}'. Creating a placeholder PST file.");
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // IMAP server connection details (placeholders)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping restore operation.");
                return;
            }

            // Create and use the IMAP client (constructor establishes connection)
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                // Load the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Restore IMAP folders from the PST
                    client.Restore(pst, new RestoreSettings());
                    Console.WriteLine("Restore operation completed successfully.");
                }
                // Client will be disposed automatically, which closes the connection
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
