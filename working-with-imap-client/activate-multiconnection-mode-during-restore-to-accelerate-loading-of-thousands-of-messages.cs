using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;

namespace ImapRestoreExample
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // Placeholder connection details – real credentials should be provided by the user.
                string imapHost = "imap.example.com";
                string imapUsername = "user@example.com";
                string imapPassword = "password";
                string pstFilePath = "backup.pst";

                // Skip real network calls when placeholder credentials are used.
                if (imapHost.Contains("example.com") || imapUsername.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected – skipping network operations.");
                    return;
                }

                // Guard PST file access.
                if (!File.Exists(pstFilePath))
                {
                    // Create a minimal placeholder PST file.
                    try
                    {
                        PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Placeholder PST created at '{pstFilePath}'.");
                    }
                    catch (Exception pstCreateEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST: {pstCreateEx.Message}");
                    }
                    return;
                }

                // Load the PST file inside a using block.
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Create and configure the IMAP client.
                    using (ImapClient client = new ImapClient(imapHost, imapUsername, imapPassword, SecurityOptions.Auto))
                    {
                        try
                        {
                            // Activate MultiConnection mode for heavy‑load restore operations.
                            client.UseMultiConnection = MultiConnectionMode.Enable;

                            // Perform the restore using the correct signature.
                            client.Restore(pst, new RestoreSettings());

                            Console.WriteLine("Restore operation completed successfully.");
                        }
                        catch (Exception clientEx)
                        {
                            Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
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
