using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string pstPath = "archive.pst";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure PST file exists
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Prepare the source folder (Inbox) information
                ExchangeFolderInfo inboxInfo;
                try
                {
                    inboxInfo = client.GetFolderInfo(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Inbox folder info: {ex.Message}");
                    return;
                }

                // Create a collection with the folder to backup
                ExchangeFolderInfoCollection foldersToBackup = new ExchangeFolderInfoCollection();
                foldersToBackup.Add(inboxInfo);

                // Open PST file stream for writing
                using (FileStream pstStream = new FileStream(pstPath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    // Backup the folder contents to the PST file
                    try
                    {
                        BackupOptions backupOptions = new BackupOptions();
                        client.Backup(foldersToBackup, pstStream, backupOptions);
                        Console.WriteLine("Inbox successfully archived to PST.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Backup failed: {ex.Message}");
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
