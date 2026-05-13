using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "input.pst";
            string outputRoot = "ExportedEmails";

            // Verify input PST file exists
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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder
                    ProcessFolder(pst, pst.RootFolder, outputRoot);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputBasePath)
    {
        // Build directory path for this folder
        string folderPath = Path.Combine(outputBasePath, SanitizePathComponent(folder.DisplayName));

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create folder '{folderPath}': {ex.Message}");
            return;
        }

        // Export messages in this folder
        int messageIndex = 0;
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            messageIndex++;
            string fileName = GenerateFileName(messageInfo.Subject, messageIndex);
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string body = message.Body ?? string.Empty;
                    File.WriteAllText(filePath, body);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to export message '{messageInfo.Subject}': {ex.Message}");
                // Continue with next message
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, folderPath);
        }
    }

    private static string GenerateFileName(string subject, int index)
    {
        string baseName = string.IsNullOrWhiteSpace(subject) ? $"Message_{index}" : SanitizePathComponent(subject);
        return $"{baseName}.txt";
    }

    private static string SanitizePathComponent(string component)
    {
        if (string.IsNullOrEmpty(component))
        {
            return "Unnamed";
        }

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            component = component.Replace(c.ToString(), "_");
        }

        // Trim to reasonable length
        if (component.Length > 100)
        {
            component = component.Substring(0, 100);
        }

        return component;
    }
}
