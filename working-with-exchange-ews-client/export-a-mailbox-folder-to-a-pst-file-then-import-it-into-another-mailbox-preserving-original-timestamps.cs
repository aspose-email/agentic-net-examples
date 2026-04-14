using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Source mailbox credentials
            string sourceMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string sourceUsername = "sourceUser";
            string sourcePassword = "sourcePass";

            // Target mailbox credentials
            string targetMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string targetUsername = "targetUser";
            string targetPassword = "targetPass";

            // Folder to export (e.g., Inbox)
            string folderName = "Inbox";

            // PST file path
            string pstPath = "backup.pst";

            // Ensure PST file exists (create minimal placeholder if missing)
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST placeholder: {ex.Message}");
                    return;
                }
            }

            // ---------- Export folder to PST ----------
            using (IEWSClient sourceClient = EWSClient.GetEWSClient(sourceMailboxUri, sourceUsername, sourcePassword))
            {
                try
                {
                    // Retrieve folder information
                    ExchangeFolderInfo folderInfo = sourceClient.GetFolderInfo(folderName);

                    // Prepare collection of folders to backup
                    ExchangeFolderInfoCollection folders = new ExchangeFolderInfoCollection();
                    folders.Add(folderInfo);

                    // Perform backup to PST file
                    sourceClient.Backup(folders, pstPath, new BackupOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Export failed: {ex.Message}");
                    return;
                }
            }

            // ---------- Import PST into target mailbox ----------
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            using (IEWSClient targetClient = EWSClient.GetEWSClient(targetMailboxUri, targetUsername, targetPassword))
            {
                try
                {
                    // Restore folders from PST (preserves original timestamps by default)
                    targetClient.Restore(pst, new RestoreSettings());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Import failed: {ex.Message}");
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
