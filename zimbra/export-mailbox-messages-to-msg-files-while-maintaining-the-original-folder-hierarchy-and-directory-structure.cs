using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "storage.pst";
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

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputPath)
    {
        string folderPath = Path.Combine(outputPath, SanitizePath(folder.DisplayName));
        try
        {
            Directory.CreateDirectory(folderPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating folder '{folderPath}': {ex.Message}");
            return;
        }

        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                {
                    string subject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                    string fileName = SanitizePath(subject) + ".msg";
                    string filePath = Path.Combine(folderPath, fileName);
                    msg.Save(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message in folder '{folder.DisplayName}': {ex.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputPath);
        }
    }

    private static string SanitizePath(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}