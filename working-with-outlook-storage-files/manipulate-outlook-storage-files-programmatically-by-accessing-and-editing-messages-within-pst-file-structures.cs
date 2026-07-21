using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Directory to save extracted messages
            const string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve total items count
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");

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

                        // Extract the full message object as MapiMessage
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        // Sanitize subject for filename
                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        try
                        {
                            // Save the message as a .msg file
                            msg.Save(outputPath);
                            Console.WriteLine($"Saved message to: {outputPath}");
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
