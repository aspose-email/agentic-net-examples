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
            string outputRoot = "output";

            // Guard PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output root directory exists
            try
            {
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                ProcessFolder(pst, rootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputRoot)
    {
        // Build the corresponding output directory path
        string folderRelativePath;
        try
        {
            folderRelativePath = folder.RetrieveFullPath();
        }
        catch
        {
            // Fallback to folder display name if RetrieveFullPath fails
            folderRelativePath = folder.DisplayName;
        }

        string targetFolderPath = Path.Combine(outputRoot, folderRelativePath);
        try
        {
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
        }
        catch (Exception dirEx)
        {
            Console.Error.WriteLine($"Failed to create folder '{targetFolderPath}': {dirEx.Message}");
            return;
        }

        // Export each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // Create a safe filename
                    string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }

                    string fileName = $"{subject}_{messageInfo.EntryIdString}.eml";
                    string filePath = Path.Combine(targetFolderPath, fileName);

                    // Save as EML
                    message.Save(filePath);
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to export message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.EnumerateFolders())
        {
            ProcessFolder(pst, subFolder, outputRoot);
        }
    }
}
