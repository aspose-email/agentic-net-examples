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
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating output directory – {dirEx.Message}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst.RootFolder, outputRoot, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, string currentPath, PersonalStorage pst)
    {
        string safeFolderName = GetSafeFileName(folder.DisplayName);
        string folderPath = Path.Combine(currentPath, safeFolderName);

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
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    string subject = mapiMessage.Subject ?? "NoSubject";
                    string safeSubject = GetSafeFileName(subject);
                    string msgFilePath = Path.Combine(folderPath, $"{safeSubject}.msg");
                    mapiMessage.Save(msgFilePath);
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Error processing message in folder '{folder.DisplayName}': {msgEx.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, folderPath, pst);
        }
    }

    private static string GetSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}