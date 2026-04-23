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
            const string outputDirectory = "ExtractedAttachments";
            const long maxAttachmentSize = 5 * 1024 * 1024; // 5 MB

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception pstCreateEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {pstCreateEx.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all folders recursively
                    ProcessFolder(pst.RootFolder, pst, outputDirectory, maxAttachmentSize);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputDir, long maxSize)
    {
        // Process messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract attachments for the current message
                MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);
                foreach (MapiAttachment attachment in attachments)
                {
                    // Load attachment into memory to check its size
                    using (MemoryStream ms = new MemoryStream())
                    {
                        attachment.Save(ms);
                        long attachmentSize = ms.Length;

                        if (attachmentSize <= maxSize)
                        {
                            // Save attachment to disk
                            string safeFileName = Path.GetFileName(attachment.FileName);
                            if (string.IsNullOrEmpty(safeFileName))
                            {
                                safeFileName = "attachment.bin";
                            }
                            string destinationPath = Path.Combine(outputDir, safeFileName);
                            try
                            {
                                // Reset stream position before saving to file
                                ms.Position = 0;
                                using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                                {
                                    ms.CopyTo(fileStream);
                                }
                                Console.WriteLine($"Saved attachment: {destinationPath} ({attachmentSize} bytes)");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {saveEx.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Skipped attachment '{attachment.FileName}' (size {attachmentSize} bytes exceeds limit).");
                        }
                    }
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Error extracting attachments from message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, outputDir, maxSize);
        }
    }
}
