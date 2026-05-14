using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstAttachmentReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstFilePath = "sample.pst";

                // Create a placeholder PST if it does not exist
                if (!File.Exists(pstFilePath))
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at: {pstFilePath}");
                }

                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    ProcessFolder(pst.RootFolder, pst);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
        {
            // Process messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                // Load the message to access its attachments
                MapiMessage message = pst.ExtractMessage(messageInfo);
                MapiAttachmentCollection attachments = message.Attachments;

                foreach (MapiAttachment attachment in attachments)
                {
                    long attachmentSize = GetAttachmentSize(attachment);
                    const long sizeThreshold = 5L * 1024 * 1024; // 5 MB

                    if (attachmentSize > sizeThreshold)
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName}");
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"Attachment: {attachment.FileName}");
                        Console.WriteLine($"Size (bytes): {attachmentSize}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst);
            }
        }

        private static long GetAttachmentSize(MapiAttachment attachment)
        {
            // Save attachment to a memory stream to determine its size
            using (MemoryStream ms = new MemoryStream())
            {
                attachment.Save(ms);
                return ms.Length;
            }
        }
    }
}
