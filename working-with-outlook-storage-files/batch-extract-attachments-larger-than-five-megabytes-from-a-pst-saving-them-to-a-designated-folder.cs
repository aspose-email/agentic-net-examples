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
            const string pstPath = "sample.pst";
            const string outputDirectory = "ExtractedAttachments";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst.RootFolder, pst, outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputDir)
    {
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);
                    foreach (MapiAttachment attachment in attachments)
                    {
                        long sizeInBytes = attachment.BinaryData != null ? attachment.BinaryData.Length : 0;
                        const long fiveMegabytes = 5L * 1024 * 1024;
                        if (sizeInBytes > fiveMegabytes)
                        {
                            string safeFileName = string.IsNullOrEmpty(attachment.FileName) ? "UnnamedAttachment" : attachment.FileName;
                            string destinationPath = Path.Combine(outputDir, safeFileName);
                            try
                            {
                                attachment.Save(destinationPath);
                                Console.WriteLine($"Saved attachment: {destinationPath} ({sizeInBytes} bytes)");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {saveEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {msgEx.Message}");
                }
            }

            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst, outputDir);
            }
        }
        catch (Exception folderEx)
        {
            Console.Error.WriteLine($"Failed to process folder '{folder.DisplayName}': {folderEx.Message}");
        }
    }
}
