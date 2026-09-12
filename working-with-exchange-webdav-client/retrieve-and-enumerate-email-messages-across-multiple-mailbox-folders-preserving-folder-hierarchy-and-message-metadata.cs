using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    // Author: Aspose.Email example – extracts all messages from a PST preserving folder hierarchy.
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";
            const string outputRoot = "ExtractedMessages";

            // Guard input PST file existence.
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputRoot))
                Directory.CreateDirectory(outputRoot);

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                Console.WriteLine($"Total items count: {pst.Store.GetTotalItemsCount()}");

                // Process the root folder and all subfolders recursively.
                ProcessFolder(pst, pst.RootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively extracts messages from a folder, preserving hierarchy.
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputBasePath)
    {
        // Create a subdirectory for the current folder.
        string folderPath = Path.Combine(outputBasePath, SanitizePath(folder.DisplayName));
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        Console.WriteLine($"Folder: {folder.DisplayName}");
        Console.WriteLine($"Total items: {folder.ContentCount}");
        Console.WriteLine($"Total unread items: {folder.ContentUnreadCount}");

        // Extract each message in the current folder.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            Console.WriteLine($"Subject: {messageInfo.Subject}");

            // Extract the full message object as MapiMessage.
            MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

            // Build a safe filename from the subject.
            string safeSubject = string.IsNullOrWhiteSpace(messageInfo.Subject) ? "NoSubject" : SanitizePath(messageInfo.Subject);
            string filePath = Path.Combine(folderPath, $"{safeSubject}.msg");

            // Save the message.
            mapiMsg.Save(filePath);
        }

        // Recurse into subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, folderPath);
        }
    }

    // Removes characters that are invalid in file or directory names.
    private static string SanitizePath(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        foreach (char c in Path.GetInvalidPathChars())
            name = name.Replace(c, '_');
        return name;
    }
}
