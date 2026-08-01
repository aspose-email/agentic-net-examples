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
            const string outputFolder = "ExtractedMessages";

            // Verify input PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Input PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Open PST storage
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

                        // Extract the full MapiMessage object
                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                        // Build a safe file name from the subject
                        string safeSubject = SanitizeFileName(mapiMsg.Subject);
                        if (string.IsNullOrWhiteSpace(safeSubject))
                        {
                            safeSubject = "Untitled";
                        }

                        string outputPath = Path.Combine(outputFolder, $"{safeSubject}.msg");

                        // Save the message as .msg
                        mapiMsg.Save(outputPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Replaces characters that are invalid in file names with an underscore
    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
