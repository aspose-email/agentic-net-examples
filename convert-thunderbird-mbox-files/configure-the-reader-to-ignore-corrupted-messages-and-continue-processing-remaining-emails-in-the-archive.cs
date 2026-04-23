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
            // Paths
            string pstPath = "archive.pst";
            string outputRoot = "ExtractedMessages";

            // Ensure output directory exists
            if (!Directory.Exists(outputRoot))
            {
                Directory.CreateDirectory(outputRoot);
            }

            // Verify PST file exists before attempting to open
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder
                ProcessFolder(pst.RootFolder, outputRoot, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively process a folder and its subfolders
    private static void ProcessFolder(FolderInfo folder, string outputPath, PersonalStorage pst)
    {
        // Create a subdirectory for this folder
        string folderPath = Path.Combine(outputPath, MakeValidFileName(folder.DisplayName));
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract the message; this may throw if the message is corrupted
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // Build a safe filename from the subject
                    string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                    string safeFileName = MakeValidFileName(subject) + ".msg";
                    string filePath = Path.Combine(folderPath, safeFileName);

                    // Save the message
                    message.Save(filePath);
                    Console.WriteLine($"Saved: {filePath}");
                }
            }
            catch (Exception ex)
            {
                // Log the error and continue with the next message
                Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, folderPath, pst);
        }
    }

    // Helper to create a filesystem‑safe name
    private static string MakeValidFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
