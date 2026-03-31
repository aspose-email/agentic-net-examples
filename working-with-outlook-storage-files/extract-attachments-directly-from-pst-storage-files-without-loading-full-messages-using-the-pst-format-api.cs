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
            string pstPath = "sample.pst";
            string outputDirectory = "Attachments";

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a minimal placeholder PST if the input file does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional content needed for placeholder
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process messages in the root folder
                ProcessFolder(pst.RootFolder, pst, outputDirectory);

                // Process each subfolder recursively
                foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(subFolder, pst, outputDirectory);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputDirectory)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract attachments without loading the full message
                MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);
                foreach (MapiAttachment attachment in attachments)
                {
                    string fileName = !string.IsNullOrEmpty(attachment.LongFileName)
                        ? attachment.LongFileName
                        : attachment.FileName;

                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "attachment.bin";
                    }

                    string outputPath = Path.Combine(outputDirectory, fileName);
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{fileName}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error extracting attachments for message '{messageInfo.Subject}': {ex.Message}");
            }
        }
    }
}
