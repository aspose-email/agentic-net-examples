using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    // Author: Aspose.Email example - PST conversion with body truncation
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";
            const int maxBodyLength = 500; // characters

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "ExtractedMessages";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the full message as MapiMessage
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        // Truncate body if it exceeds the specified length
                        if (!string.IsNullOrEmpty(msg.Body) && msg.Body.Length > maxBodyLength)
                        {
                            msg.Body = msg.Body.Substring(0, maxBodyLength) + "...";
                        }

                        // Build a safe filename from the subject
                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        // Ensure filename is not too long
                        int maxFileNameLength = 200;
                        if (safeSubject.Length > maxFileNameLength)
                            safeSubject = safeSubject.Substring(0, maxFileNameLength);

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        // Save the message as .msg
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
