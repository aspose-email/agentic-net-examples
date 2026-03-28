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
            string pstPath = "storage.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            // Open PST inside a using block to ensure disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folder.DisplayName}");
                    Console.WriteLine($"Total items: {folder.ContentCount}");
                    Console.WriteLine($"Unread items: {folder.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");

                        // Extract the full MapiMessage
                        using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                        {
                            // Ensure a valid filename (replace invalid path chars)
                            string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string outputPath = $"{safeSubject}.msg";

                            // Save as MSG file (default format)
                            msg.Save(outputPath);
                            Console.WriteLine($"Saved to: {outputPath}");
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
