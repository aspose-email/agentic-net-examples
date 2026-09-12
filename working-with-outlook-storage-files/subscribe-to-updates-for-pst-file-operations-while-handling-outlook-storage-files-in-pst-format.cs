using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;


class Program
{
    static void Main()
    {
        try
        {
            const string pstFilePath = "storage.pst";
            const string outputDirectory = "ExtractedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Create a minimal PST file if it does not exist
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Initial processing of the PST file
            ProcessPst(pstFilePath, outputDirectory);

            // Set up a watcher to subscribe to PST file changes
            var watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(Path.GetFullPath(pstFilePath)) ?? ".",
                Filter = Path.GetFileName(pstFilePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size
            };

            watcher.Changed += (s, e) =>
            {
                Console.WriteLine($"PST file changed: {e.ChangeType}");
                ProcessPst(pstFilePath, outputDirectory);
            };
            watcher.Created += (s, e) =>
            {
                Console.WriteLine($"PST file created: {e.ChangeType}");
                ProcessPst(pstFilePath, outputDirectory);
            };
            watcher.Deleted += (s, e) =>
            {
                Console.WriteLine($"PST file deleted: {e.ChangeType}");
            };
            watcher.Renamed += (s, e) =>
            {
                Console.WriteLine($"PST file renamed: {e.OldFullPath} -> {e.FullPath}");
                ProcessPst(pstFilePath, outputDirectory);
            };

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            watcher.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessPst(string pstPath, string outputDir)
    {
        try
        {
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");

                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the message as a MapiMessage and convert to MailMessage
                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);
                        MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                        // Build a safe filename
                        string safeSubject = string.IsNullOrWhiteSpace(mailMsg.Subject) ? "NoSubject" : mailMsg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string messageFilePath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        // Save as .msg
                        mailMsg.Save(messageFilePath, SaveOptions.DefaultMsgUnicode);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing PST file '{pstPath}': {ex.Message}");
        }
    }
}
