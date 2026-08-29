using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        string pstPath = "sample.pst";
        string outputDir = "output";

        // Verify input PST file and prepare output directory
        try
        {
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"File system error: {ex.Message}");
            return;
        }

        // Process the PST archive
        try
        {
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(pst, folderInfo, outputDir);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folderInfo, string outputDir)
    {
        // Export each message in the current folder
        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
        {
            try
            {
                MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);
                using (MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions()))
                {
                    string safeSubject = string.IsNullOrWhiteSpace(mailMsg.Subject) ? "NoSubject" : MakeValidFileName(mailMsg.Subject);
                    string outputPath = Path.Combine(outputDir, $"{safeSubject}.mhtml");

                    // Ensure unique file name
                    int counter = 1;
                    while (File.Exists(outputPath))
                    {
                        outputPath = Path.Combine(outputDir, $"{safeSubject}_{counter}.mhtml");
                        counter++;
                    }

                    // Save as MHTML; format inferred from extension
                    mailMsg.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to export message '{messageInfo.Subject}': {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folderInfo.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDir);
        }
    }

    static string MakeValidFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
