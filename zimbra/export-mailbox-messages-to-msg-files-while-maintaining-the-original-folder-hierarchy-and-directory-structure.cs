using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input PST file path (adjust as needed)
            string pstPath = "mailbox.pst";
            // Output root directory for exported MSG files
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
                // Process each folder recursively
                ProcessFolder(pst.RootFolder, outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively process a PST folder and export its messages
    private static void ProcessFolder(FolderInfo folder, string currentPath)
    {
        // Create directory for this folder
        string folderPath = Path.Combine(currentPath, SanitizePathComponent(folder.DisplayName));
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Export all messages in this folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
            {
                string subject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                string fileName = SanitizePathComponent(subject) + ".msg";
                string filePath = Path.Combine(folderPath, fileName);

                // Save as MSG (no SaveOptions needed)
                msg.Save(filePath);
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, folderPath);
        }
    }

    // Helper to remove invalid path characters
    private static string SanitizePathComponent(string component)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            component = component.Replace(c.ToString(), "_");
        }
        foreach (char c in Path.GetInvalidPathChars())
        {
            component = component.Replace(c.ToString(), "_");
        }
        return component;
    }

    // Reference to the opened PST for message extraction
    private static PersonalStorage pst;
}