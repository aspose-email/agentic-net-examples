using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        const string pstPath = "updates.pst";
        const string outputDir = "ExtractedMessages";

        // Ensure output directory exists
        try
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Verify PST file existence
        if (!File.Exists(pstPath))
        {
            Console.Error.WriteLine($"PST file not found at path: {pstPath}");
            return;
        }

        try
        {
            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the full MAPI message
                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                        // Build a safe filename from the subject
                        string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');
                        if (safeSubject.Length > 100)
                            safeSubject = safeSubject.Substring(0, 100);

                        string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        // Ensure unique filename
                        int duplicateIndex = 1;
                        while (File.Exists(msgPath))
                        {
                            msgPath = Path.Combine(outputDir, $"{safeSubject}_{duplicateIndex}.msg");
                            duplicateIndex++;
                        }

                        // Save the message as .msg
                        mapiMsg.Save(msgPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while processing the PST file: {ex.Message}");
        }
    }
}
