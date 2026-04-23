using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "storage.pst";
            string outputRoot = "ExtractedMessages";

            // Guard PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
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
                    FolderInfo rootFolder = pst.RootFolder;
                    ProcessFolder(rootFolder, outputRoot, pst);
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

    private static void ProcessFolder(FolderInfo folder, string currentPath, PersonalStorage pst)
    {
        // Build path for this folder
        string folderPath = Path.Combine(currentPath, GetSafeFolderName(folder.DisplayName));

        // Ensure folder path exists
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

        // Process messages in this folder
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                // Extract full message
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string safeSubject = GetSafeFileName(message.Subject);
                    if (string.IsNullOrWhiteSpace(safeSubject))
                    {
                        safeSubject = "Untitled";
                    }

                    string messageFilePath = Path.Combine(folderPath, $"{safeSubject}.msg");

                    // Save message to disk
                    try
                    {
                        message.Save(messageFilePath);
                        Console.WriteLine($"Saved: {messageFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message '{message.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating messages in folder '{folder.DisplayName}': {ex.Message}");
        }

        // Recurse into subfolders
        try
        {
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, folderPath, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating subfolders of '{folder.DisplayName}': {ex.Message}");
        }
    }

    // Removes characters that are invalid in Windows file/folder names
    private static string GetSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c.ToString(), "_");
        }
        return name.Trim();
    }

    // Removes characters that are invalid in Windows folder names
    private static string GetSafeFolderName(string name)
    {
        foreach (char c in Path.GetInvalidPathChars())
        {
            name = name.Replace(c.ToString(), "_");
        }
        return name.Trim();
    }
}
