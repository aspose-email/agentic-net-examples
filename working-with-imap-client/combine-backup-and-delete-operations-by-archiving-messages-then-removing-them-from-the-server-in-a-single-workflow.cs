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
            // Placeholder credentials – skip execution if they are not replaced.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";
            string backupFilePath = "backup.pst";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Ensure backup file exists or create a minimal PST placeholder.
            try
            {
                if (!File.Exists(backupFilePath))
                {
                    PersonalStorage.Create(backupFilePath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare backup file: {ex.Message}");
                return;
            }

            // Connect to IMAP server and perform backup + delete.
            try
            {
                using (ImapClient imapClient = new ImapClient(host, username, password))
                {
                    // Select the target folder.
                    imapClient.SelectFolder(folderName);

                    // Prepare folder collection for backup.
                    ImapFolderInfoCollection foldersToBackup = new ImapFolderInfoCollection();
                    foldersToBackup.Add(imapClient.GetFolderInfo(folderName));

                    // Backup the folder to the PST file.
                    try
                    {
                        imapClient.Backup(foldersToBackup, backupFilePath, new BackupSettings());
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Backup failed: {ex.Message}");
                        return;
                    }

                    // Retrieve all messages in the folder.
                    ImapMessageInfoCollection messages = imapClient.ListMessages();

                    // Delete the retrieved messages.
                    try
                    {
                        if (messages != null && messages.Count > 0)
                        {
                            imapClient.DeleteMessages(messages);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Deletion failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
