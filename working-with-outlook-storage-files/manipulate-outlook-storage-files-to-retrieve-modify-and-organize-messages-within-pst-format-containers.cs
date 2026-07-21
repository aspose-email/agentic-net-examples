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
            const string outputDir = "output";

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Ensure the PST file exists; create a minimal empty PST if missing.
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve and display total items count.
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");

                // Iterate through each subfolder of the root folder.
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder.
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the full message as a MapiMessage.
                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                        // Prepare a safe filename based on the subject.
                        string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        // Ensure unique filename.
                        string msgFilePath = Path.Combine(outputDir, $"{safeSubject}.msg");
                        int duplicateIndex = 1;
                        while (File.Exists(msgFilePath))
                        {
                            msgFilePath = Path.Combine(outputDir, $"{safeSubject}_{duplicateIndex}.msg");
                            duplicateIndex++;
                        }

                        // Save the message as a .msg file.
                        mapiMsg.Save(msgFilePath);
                        Console.WriteLine($"Saved message to '{msgFilePath}'.");
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
