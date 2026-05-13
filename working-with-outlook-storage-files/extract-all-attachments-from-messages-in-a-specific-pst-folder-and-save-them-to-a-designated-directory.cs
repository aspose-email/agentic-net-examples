using System;
using System.IO;
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
            string targetFolderName = "Inbox";
            string outputDirectory = "ExtractedAttachments";

            // Guard PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
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

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Locate the target folder
                FolderInfo targetFolder;
                try
                {
                    targetFolder = pst.RootFolder.GetSubFolder(targetFolderName);
                }
                catch (Exception folderEx)
                {
                    Console.Error.WriteLine($"Failed to locate folder '{targetFolderName}': {folderEx.Message}");
                    return;
                }

                if (targetFolder == null)
                {
                    Console.Error.WriteLine($"Folder '{targetFolderName}' not found in PST.");
                    return;
                }

                // Enumerate messages in the folder
                foreach (MessageInfo messageInfo in targetFolder.EnumerateMessages())
                {
                    // Extract attachments for the current message
                    MapiAttachmentCollection attachments;
                    try
                    {
                        attachments = pst.ExtractAttachments(messageInfo);
                    }
                    catch (Exception attachEx)
                    {
                        Console.Error.WriteLine($"Failed to extract attachments for message '{messageInfo.Subject}': {attachEx.Message}");
                        continue;
                    }

                    // Save each attachment
                    foreach (MapiAttachment attachment in attachments)
                    {
                        string fileName = attachment.FileName;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = Guid.NewGuid().ToString();
                        }

                        string outputPath = Path.Combine(outputDirectory, fileName);
                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved attachment: {outputPath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{fileName}': {saveEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
