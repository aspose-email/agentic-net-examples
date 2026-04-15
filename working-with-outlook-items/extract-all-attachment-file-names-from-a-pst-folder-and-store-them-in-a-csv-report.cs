using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path
            string pstPath = "sample.pst";
            // Folder name inside the PST to scan (e.g., "Inbox")
            string folderName = "Inbox";
            // Output CSV report path
            string csvPath = "AttachmentReport.csv";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                try
                {
                    Directory.CreateDirectory(csvDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Locate the target folder
                FolderInfo targetFolder;
                try
                {
                    targetFolder = pst.RootFolder.GetSubFolder(folderName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to locate folder '{folderName}': {ex.Message}");
                    return;
                }

                // Prepare CSV writer
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // Write CSV header
                    writer.WriteLine("MessageSubject,AttachmentFileName");

                    // Enumerate messages in the folder
                    foreach (MessageInfo messageInfo in targetFolder.EnumerateMessages())
                    {
                        // Extract attachments for the current message
                        MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);

                        // If there are no attachments, continue
                        if (attachments == null || attachments.Count == 0)
                            continue;

                        // Write each attachment file name to CSV
                        foreach (MapiAttachment attachment in attachments)
                        {
                            // Escape commas in subject or filename if needed
                            string safeSubject = messageInfo.Subject?.Replace(",", " ");
                            string safeFileName = attachment.FileName?.Replace(",", " ");
                            writer.WriteLine($"{safeSubject},{safeFileName}");
                        }
                    }
                }

                Console.WriteLine($"Attachment report generated at: {csvPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
