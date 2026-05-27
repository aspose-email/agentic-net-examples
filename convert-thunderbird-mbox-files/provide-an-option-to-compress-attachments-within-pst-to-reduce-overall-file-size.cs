using Aspose.Email.Mapi;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
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
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open PST for read/write
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    string attachmentOutputDir = "ExtractedAttachments";
                    try
                    {
                        if (!Directory.Exists(attachmentOutputDir))
                        {
                            Directory.CreateDirectory(attachmentOutputDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }

                    ProcessFolder(pst.RootFolder, pst, attachmentOutputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputDir)
    {
        // Process messages in the current folder
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                ProcessMessage(messageInfo, pst, outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to enumerate messages in folder '{folder.DisplayName}': {ex.Message}");
        }

        // Recursively process subfolders
        try
        {
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst, outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to enumerate subfolders in folder '{folder.DisplayName}': {ex.Message}");
        }
    }

    private static void ProcessMessage(MessageInfo messageInfo, PersonalStorage pst, string outputDir)
    {
        try
        {
            using (MapiMessage message = pst.ExtractMessage(messageInfo))
            {
                // Iterate over each attachment
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string originalFilePath = Path.Combine(outputDir, attachment.FileName);
                    try
                    {
                        // Save the original attachment to disk
                        attachment.Save(originalFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                        continue;
                    }

                    // Compress the saved attachment using GZip
                    string compressedFilePath = originalFilePath + ".gz";
                    try
                    {
                        using (FileStream originalFileStream = new FileStream(originalFilePath, FileMode.Open, FileAccess.Read))
                        using (FileStream compressedFileStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write))
                        using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(gzipStream);
                        }

                        // Optionally delete the original uncompressed file
                        try
                        {
                            File.Delete(originalFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete original attachment file '{originalFilePath}': {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to compress attachment '{attachment.FileName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {ex.Message}");
        }
    }
}
