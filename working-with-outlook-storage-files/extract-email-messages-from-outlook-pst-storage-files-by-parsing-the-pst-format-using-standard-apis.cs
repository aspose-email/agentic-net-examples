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
            string pstPath = "sample.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder in the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        // Extract the full message
                        using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                        {
                            // Save the message as a .msg file
                            try
                            {
                                string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Untitled" : msg.Subject;
                                // Replace invalid filename characters
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }
                                string outputPath = $"{safeSubject}.msg";
                                msg.Save(outputPath);
                                Console.WriteLine($"Saved: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error saving message: {ex.Message}");
                            }
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
