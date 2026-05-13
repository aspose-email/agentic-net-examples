using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP backup verification.");
                return;
            }

            // Ensure backup directory exists
            string backupFilePath = "imap_backup.pst";
            string backupDirectory = Path.GetDirectoryName(backupFilePath);
            if (!string.IsNullOrEmpty(backupDirectory) && !Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select INBOX folder
                    client.SelectFolder("INBOX");

                    // Count messages before backup
                    ImapMessageInfoCollection messagesBefore = client.ListMessages();
                    int countBefore = messagesBefore.Count;

                    // Prepare folder collection for backup (INBOX only)
                    ImapFolderInfo inboxInfo = client.GetFolderInfo("INBOX");
                    ImapFolderInfoCollection foldersToBackup = new ImapFolderInfoCollection();
                    foldersToBackup.Add(inboxInfo);

                    // Perform backup
                    client.Backup(foldersToBackup, backupFilePath, new BackupSettings());

                    // Count messages after backup
                    client.SelectFolder("INBOX"); // Re-select to ensure folder is active
                    ImapMessageInfoCollection messagesAfter = client.ListMessages();
                    int countAfter = messagesAfter.Count;

                    // Verify counts
                    if (countBefore == countAfter)
                    {
                        Console.WriteLine($"Backup verified successfully. Message count before and after backup: {countBefore}");
                    }
                    else
                    {
                        Console.WriteLine($"Backup verification failed. Count before: {countBefore}, count after: {countAfter}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
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
