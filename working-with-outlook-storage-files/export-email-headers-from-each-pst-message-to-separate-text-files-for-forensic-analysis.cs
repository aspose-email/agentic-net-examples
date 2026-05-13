using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths (replace with actual paths as needed)
            string pstPath = "sample.pst";
            string outputDirectory = "Headers";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst.RootFolder, outputDirectory, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folderInfo, string outputDirectory, PersonalStorage pst)
    {
        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
        {
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string headers = message.Headers?.ToString() ?? string.Empty;

                    string safeSubject = MakeSafeFileName(message.Subject);
                    if (string.IsNullOrEmpty(safeSubject))
                    {
                        safeSubject = "Message_" + messageInfo.EntryIdString;
                    }

                    string headerFilePath = Path.Combine(outputDirectory, safeSubject + ".txt");
                    File.WriteAllText(headerFilePath, headers);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryIdString}: {ex.Message}");
            }
        }

        foreach (FolderInfo subFolder in folderInfo.GetSubFolders())
        {
            ProcessFolder(subFolder, outputDirectory, pst);
        }
    }

    private static string MakeSafeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        if (name.Length > 100)
            name = name.Substring(0, 100);

        return name;
    }
}
