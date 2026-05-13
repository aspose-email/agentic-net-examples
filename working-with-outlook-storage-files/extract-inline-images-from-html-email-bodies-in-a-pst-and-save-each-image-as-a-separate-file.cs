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
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            string outputDirectory = "ExtractedImages";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst, pst.RootFolder, outputDirectory);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error accessing PST file: {pstEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
    {
        // Enumerate messages in the current folder
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        // Check if the message has an HTML body
                        string htmlBody = message.BodyHtml;
                        if (string.IsNullOrEmpty(htmlBody))
                            continue;

                        // Save each inline attachment (typically images)
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            // Inline attachments are marked with IsInline flag
                            if (!attachment.IsInline)
                                continue;

                            string safeFileName = attachment.FileName;
                            if (string.IsNullOrEmpty(safeFileName))
                            {
                                safeFileName = Guid.NewGuid().ToString() + ".bin";
                            }

                            string outputPath = Path.Combine(outputDirectory, safeFileName);
                            try
                            {
                                attachment.Save(outputPath);
                                Console.WriteLine($"Saved inline image: {outputPath}");
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
        }
        catch (Exception enumEx)
        {
            Console.Error.WriteLine($"Failed to enumerate messages in folder '{folder.DisplayName}': {enumEx.Message}");
        }

        // Recursively process subfolders
        try
        {
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder, outputDirectory);
            }
        }
        catch (Exception subEx)
        {
            Console.Error.WriteLine($"Failed to enumerate subfolders in folder '{folder.DisplayName}': {subEx.Message}");
        }
    }
}
