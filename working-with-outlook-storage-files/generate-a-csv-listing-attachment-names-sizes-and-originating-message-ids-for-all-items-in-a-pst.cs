using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";
            const string csvPath = "attachments.csv";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string csvDir = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(csvDir) && !Directory.Exists(csvDir))
                {
                    Directory.CreateDirectory(csvDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Open PST and CSV writer
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            using (StreamWriter writer = new StreamWriter(csvPath, false))
            {
                // Write CSV header
                writer.WriteLine("MessageId,AttachmentName,Size");

                // Process root folder and its subfolders recursively
                ProcessFolder(pst.RootFolder, pst, writer);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, StreamWriter writer)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract attachments for the current message
            MapiAttachmentCollection attachments;
            try
            {
                attachments = pst.ExtractAttachments(messageInfo);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to extract attachments for message {messageInfo.EntryIdString}: {ex.Message}");
                continue;
            }

            // Write each attachment's details to CSV
            foreach (MapiAttachment attachment in attachments)
            {
                string attachmentName = attachment.FileName ?? string.Empty;
                long size = attachment.BinaryData != null ? attachment.BinaryData.Length : 0;
                string messageId = messageInfo.EntryIdString ?? string.Empty;

                // Escape commas in fields if necessary
                string escapedName = attachmentName.Contains(",") ? $"\"{attachmentName}\"" : attachmentName;
                string escapedMessageId = messageId.Contains(",") ? $"\"{messageId}\"" : messageId;

                writer.WriteLine($"{escapedMessageId},{escapedName},{size}");
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, writer);
        }
    }
}
