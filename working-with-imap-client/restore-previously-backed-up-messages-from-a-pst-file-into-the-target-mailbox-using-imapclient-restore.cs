using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailImapRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define IMAP connection parameters (placeholders will cause early exit)
                string imapHost = "imap.example.com";
                int imapPort = 993;
                string imapUsername = "user@example.com";
                string imapPassword = "password";

                // Detect placeholder credentials and skip external call
                if (imapHost.Contains("example.com") || imapUsername.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder IMAP credentials detected. Skipping restore operation.");
                    return;
                }

                // Path to the PST file that contains the backup
                string pstPath = "backup.pst";

                // Ensure the PST file exists; create a minimal placeholder if it does not
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Placeholder PST created at '{pstPath}'. No restore performed.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    }
                    return;
                }

                // Load the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Create and use the IMAP client
                    using (ImapClient client = new ImapClient(imapHost, imapPort, imapUsername, imapPassword))
                    {
                        try
                        {
                            // Restore the folders and messages from the PST into the mailbox
                            client.Restore(pst, new RestoreSettings());
                            Console.WriteLine("Restore operation completed successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"IMAP restore failed: {ex.Message}");
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
}
