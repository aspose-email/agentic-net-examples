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
            string outputRoot = "ExportedMessages";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            try
            {
                Directory.CreateDirectory(outputRoot);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string basePath)
    {
        string folderPath = Path.Combine(basePath, SanitizePath(folder.DisplayName));
        try
        {
            Directory.CreateDirectory(folderPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating folder '{folderPath}': {ex.Message}");
            return;
        }

        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                {
                    string subject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                    string fileName = SanitizePath(subject) + ".msg";
                    string filePath = Path.Combine(folderPath, fileName);
                    mapiMsg.Save(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to export message '{msgInfo.Subject}': {ex.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, basePath);
        }
    }

    static string SanitizePath(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}