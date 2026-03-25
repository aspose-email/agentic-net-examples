using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace ExportMailboxMessages
{
    class Program
    {
        static void Main()
        {
            try
            {
                string pstPath = "input.pst";
                string outputRoot = "ExportedMessages";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(outputRoot);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                        return;
                    }
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
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }

        // Recursively processes a folder and its subfolders
        private static void ProcessFolder(FolderInfo folder, string currentPath, PersonalStorage pst)
        {
            // Create a directory for this folder
            string folderPath = Path.Combine(currentPath, GetSafeDirectoryName(folder.DisplayName));
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating folder '{folderPath}': {ex.Message}");
                return;
            }

            // Export all messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                    {
                        string subject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                        string fileName = GetSafeFileName(subject) + ".msg";
                        string filePath = Path.Combine(folderPath, fileName);
                        msg.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error exporting message in folder '{folder.DisplayName}': {ex.Message}");
                }
            }

            // Recurse into subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, folderPath, pst);
            }
        }

        // Generates a file-system safe file name
        private static string GetSafeFileName(string name)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                name = name.Replace(c, '_');
            }
            // Trim length to avoid excessively long paths
            if (name.Length > 100)
            {
                name = name.Substring(0, 100);
            }
            return name;
        }

        // Generates a file-system safe directory name
        private static string GetSafeDirectoryName(string name)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            foreach (char c in invalidChars)
            {
                name = name.Replace(c, '_');
            }
            // Trim length to avoid excessively long paths
            if (name.Length > 100)
            {
                name = name.Substring(0, 100);
            }
            return name;
        }
    }
}