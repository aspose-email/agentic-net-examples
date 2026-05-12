using Aspose.Email;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailAttachmentZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstFilePath = "sample.pst";
                string zipFilePath = "attachments.zip";

                // Verify PST file exists
                if (!File.Exists(pstFilePath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                    return;
                }

                // Ensure output directory exists
                string zipDirectory = Path.GetDirectoryName(zipFilePath);
                if (!string.IsNullOrEmpty(zipDirectory) && !Directory.Exists(zipDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(zipDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{zipDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                // Open PST and create ZIP archive
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                using (FileStream zipFileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write))
                using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                {
                    int attachmentIndex = 0;
                    ProcessFolder(pst.RootFolder, pst, zipArchive, ref attachmentIndex);
                }

                Console.WriteLine($"All attachments have been zipped to '{zipFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, ZipArchive zip, ref int attachmentIndex)
        {
            // Process messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);
                    foreach (MapiAttachment attachment in attachments)
                    {
                        string safeFileName = string.IsNullOrEmpty(attachment.FileName) ? $"attachment_{attachmentIndex}" : attachment.FileName;
                        string entryName = $"{attachmentIndex}_{safeFileName}";
                        attachmentIndex++;

                        ZipArchiveEntry zipEntry = zip.CreateEntry(entryName);
                        using (Stream entryStream = zipEntry.Open())
                        {
                            attachment.Save(entryStream);
                        }
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Failed to extract attachments from message '{messageInfo.Subject}': {msgEx.Message}");
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst, zip, ref attachmentIndex);
            }
        }
    }
}
