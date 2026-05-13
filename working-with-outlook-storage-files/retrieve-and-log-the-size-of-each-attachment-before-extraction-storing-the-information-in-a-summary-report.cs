using System;
using System.IO;
using System.Collections.Generic;
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
            string outputDir = "Attachments";
            string reportPath = "AttachmentReport.txt";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
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
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            List<string> reportLines = new List<string>();

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all folders recursively
                    Queue<FolderInfo> folders = new Queue<FolderInfo>();
                    folders.Enqueue(pst.RootFolder);

                    while (folders.Count > 0)
                    {
                        FolderInfo currentFolder = folders.Dequeue();

                        // Enqueue subfolders
                        foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                        {
                            folders.Enqueue(subFolder);
                        }

                        // Process messages in the current folder
                        foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                        {
                            // Extract attachments collection for the message
                            MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);

                            foreach (MapiAttachment attachment in attachments)
                            {
                                // Determine attachment size using BinaryData length
                                int sizeInBytes = attachment.BinaryData != null ? attachment.BinaryData.Length : 0;

                                // Log to console
                                Console.WriteLine($"Message: {messageInfo.Subject}");
                                Console.WriteLine($"  Attachment: {attachment.FileName}, Size: {sizeInBytes} bytes");

                                // Add entry to summary report
                                reportLines.Add($"Message: {messageInfo.Subject}, Attachment: {attachment.FileName}, Size: {sizeInBytes} bytes");

                                // Save attachment to disk
                                try
                                {
                                    string attachmentPath = Path.Combine(outputDir, attachment.FileName);
                                    attachment.Save(attachmentPath);
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }

            // Write summary report to file
            try
            {
                File.WriteAllLines(reportPath, reportLines);
                Console.WriteLine($"Attachment summary report written to '{reportPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write report file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
