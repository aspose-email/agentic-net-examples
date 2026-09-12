using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "storage.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file '{pstPath}' not found.");
                return;
            }

            string outputDir = "ExtractedMessages";
            if (!Directory.Exists(outputDir))
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

                        // Extract the message as a MapiMessage
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        // Create a safe filename from the subject
                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        // Save the message as a .msg file
                        msg.Save(outputPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
