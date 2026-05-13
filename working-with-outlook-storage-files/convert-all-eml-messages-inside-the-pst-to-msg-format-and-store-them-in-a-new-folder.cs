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
            string pstPath = "input.pst";
            string outputFolder = "MsgOutput";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst.RootFolder, pst, outputFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputFolder)
    {
        // Convert each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    // Build a safe file name
                    string subject = string.IsNullOrEmpty(messageInfo.Subject) ? "NoSubject" : messageInfo.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }

                    string fileName = $"{subject}_{messageInfo.EntryIdString}.msg";
                    string msgPath = Path.Combine(outputFolder, fileName);

                    // Save as MSG
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to convert message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, outputFolder);
        }
    }
}
