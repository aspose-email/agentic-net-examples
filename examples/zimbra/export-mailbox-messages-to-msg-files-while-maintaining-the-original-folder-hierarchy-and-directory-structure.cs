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
            // Input PST file path
            string pstPath = "storage.pst";
            // Output directory for exported MSG files
            string outputRoot = "ExportedMessages";

            // Guard input file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Ensure output root directory exists
            if (!Directory.Exists(outputRoot))
            {
                Directory.CreateDirectory(outputRoot);
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Start processing from the root folder
                ExportFolder(pst, pst.RootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively export all messages in a folder and its subfolders
    private static void ExportFolder(PersonalStorage pst, FolderInfo folder, string currentPath)
    {
        // Export messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    string safeSubject = GetSafeFileName(mapiMessage.Subject);
                    string msgFilePath = Path.Combine(currentPath, safeSubject + ".msg");
                    mapiMessage.Save(msgFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to export message: {ex.Message}");
            }
        }

        // Process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            string subFolderPath = Path.Combine(currentPath, subFolder.DisplayName);
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            ExportFolder(pst, subFolder, subFolderPath);
        }
    }

    // Replace invalid filename characters with underscore
    private static string GetSafeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "Untitled";
        }

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            name = name.Replace(c, '_');
        }

        // Trim length to avoid filesystem limits
        if (name.Length > 100)
        {
            name = name.Substring(0, 100);
        }

        return name;
    }
}