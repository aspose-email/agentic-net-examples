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
            // Path to the PST file
            string pstPath = "storage.pst";

            // Guard against missing file
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Define output directory for saved messages
            string outputDir = "output";
            Directory.CreateDirectory(outputDir); // Ensure the directory exists

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages within the folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"  Subject: {messageInfo.Subject}");

                        // Extract the full message as MapiMessage
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        // Prepare a safe filename
                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        // Guard against overwriting existing files
                        if (File.Exists(msgPath))
                        {
                            Console.Error.WriteLine($"File already exists, skipping: {msgPath}");
                        }
                        else
                        {
                            msg.Save(msgPath);
                            Console.WriteLine($"  Saved message to: {msgPath}");
                        }
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
