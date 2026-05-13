using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "storage.pst";
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            string outputDirectory = "output";
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

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDir)
    {
        // Process each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string body = message.Body ?? string.Empty;
                    string subject = message.Subject ?? "NoSubject";
                    string safeFileName = GetSafeFileName(subject) + ".md";
                    string filePath = Path.Combine(outputDir, safeFileName);

                    try
                    {
                        File.WriteAllText(filePath, body);
                        Console.WriteLine($"Exported: {filePath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Failed to write file '{filePath}': {writeEx.Message}");
                    }
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to extract message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDir);
        }
    }

    static string GetSafeFileName(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }

        // Trim length to avoid excessively long filenames
        const int maxLength = 100;
        if (name.Length > maxLength)
        {
            name = name.Substring(0, maxLength);
        }

        return name;
    }
}
