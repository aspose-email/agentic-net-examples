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

            // Verify input PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Ensure output root directory exists
            try
            {
                Directory.CreateDirectory(outputRoot);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Start recursive export from the root folder
                ExportFolder(pst.RootFolder, outputRoot, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ExportFolder(FolderInfo folder, string parentPath, PersonalStorage pst)
    {
        // Create a directory for the current folder
        string safeFolderName = MakeFileSystemSafe(folder.DisplayName);
        string currentPath = Path.Combine(parentPath, safeFolderName);
        try
        {
            Directory.CreateDirectory(currentPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating folder '{currentPath}': {ex.Message}");
            return;
        }

        // Export all messages in this folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                {
                    string subject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                    string safeSubject = MakeFileSystemSafe(subject);
                    string msgPath = Path.Combine(currentPath, safeSubject + ".msg");
                    msg.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error exporting message in folder '{folder.DisplayName}': {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ExportFolder(subFolder, currentPath, pst);
        }
    }

    // Helper to replace invalid file system characters
    private static string MakeFileSystemSafe(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}