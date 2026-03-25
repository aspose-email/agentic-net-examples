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
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating output directory – {dirEx.Message}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                ProcessFolder(pst, rootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputRoot)
    {
        string folderPath = Path.Combine(outputRoot, SanitizePath(folder.DisplayName));

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        catch (Exception dirEx)
        {
            Console.Error.WriteLine($"Error creating folder '{folderPath}' – {dirEx.Message}");
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
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Error processing message in folder '{folder.DisplayName}' – {msgEx.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputRoot);
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