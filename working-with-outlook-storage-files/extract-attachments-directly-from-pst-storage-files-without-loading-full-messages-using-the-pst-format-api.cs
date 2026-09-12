using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace ExtractPstAttachments
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file
                string pstPath = "storage.pst";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = "Attachments";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

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
                            Console.WriteLine($"Processing message: {messageInfo.Subject}");

                            // Extract attachments without loading the full message
                            MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);

                            foreach (MapiAttachment attachment in attachments)
                            {
                                // Replace invalid filename characters with underscore
                                string safeFileName = string.Join("_", attachment.FileName.Split(Path.GetInvalidFileNameChars()));
                                string attachmentPath = Path.Combine(outputDir, safeFileName);
                                Console.WriteLine($"Saving attachment: {attachmentPath}");
                                attachment.Save(attachmentPath);
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
}
