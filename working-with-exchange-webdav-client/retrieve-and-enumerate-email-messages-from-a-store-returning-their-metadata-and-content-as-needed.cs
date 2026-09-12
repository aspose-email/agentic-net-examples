using System;
using System.IO;
using Aspose.Email;
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
                Console.Error.WriteLine($"Error: PST file '{pstPath}' not found.");
                return;
            }

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

                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string outputFile = Path.Combine(outputDir, $"{safeSubject}.msg");

                        try
                        {
                            msg.Save(outputFile);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message '{msg.Subject}': {ex.Message}");
                        }
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
