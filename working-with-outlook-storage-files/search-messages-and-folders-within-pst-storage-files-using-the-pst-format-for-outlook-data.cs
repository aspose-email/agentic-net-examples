using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            const string outputDir = "output";
            Directory.CreateDirectory(outputDir);

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

                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                        string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "Untitled" : mapiMsg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        if (safeSubject.Length > 100)
                            safeSubject = safeSubject.Substring(0, 100);

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        try
                        {
                            mapiMsg.Save(outputPath);
                            Console.WriteLine($"Saved message to: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message '{mapiMsg.Subject}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
