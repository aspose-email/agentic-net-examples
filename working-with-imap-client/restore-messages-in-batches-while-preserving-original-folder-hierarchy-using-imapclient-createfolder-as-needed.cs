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
            // Configuration (replace with real values when needed)
            string imapHost = "imap.example.com";
            int imapPort = 993;
            string imapUsername = "user@example.com";
            string imapPassword = "password";
            string backupPstPath = "backup.pst";

            // Guard against placeholder credentials/host
            if (imapHost.Contains("example.com") || imapUsername.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping restore operation.");
                return;
            }

            // Ensure the PST backup file exists; create a minimal placeholder if missing
            if (!File.Exists(backupPstPath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(backupPstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default folder to avoid empty PST issues
                        placeholderPst.CreatePredefinedFolder("Placeholder", StandardIpmFolder.Inbox);
                    }
                    Console.WriteLine($"Created placeholder PST at '{backupPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(backupPstPath))
            {
                // Connect to the IMAP server
                try
                {
                    using (ImapClient client = new ImapClient(imapHost, imapPort, SecurityOptions.SSLImplicit))
                    {
                        client.Username = imapUsername;
                        client.Password = imapPassword;

                        // Validate connection by selecting the INBOX folder
                        client.SelectFolder("INBOX");

                        // Restore folders and messages from the PST, preserving hierarchy
                        client.Restore(pst, new RestoreSettings());

                        Console.WriteLine("Restore operation completed successfully.");
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
