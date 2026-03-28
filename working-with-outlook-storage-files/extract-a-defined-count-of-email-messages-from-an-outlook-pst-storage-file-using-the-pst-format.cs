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
            // Path to the PST file
            string pstPath = "sample.pst";

            // Verify that the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            // Directory where extracted messages will be saved
            string outputDir = "ExtractedMessages";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                return;
            }

            // Define how many messages to extract
            int maxMessages = 10;
            int extractedCount = 0;

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder and its subfolders recursively
                ProcessFolder(pst.RootFolder, outputDir, ref extractedCount, maxMessages);
            }

            Console.WriteLine($"Extraction completed. Total messages extracted: {extractedCount}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively extracts messages from a folder until the desired count is reached
    private static void ProcessFolder(FolderInfo folder, string outputDir, ref int extractedCount, int maxMessages)
    {
        // Enumerate messages in the current folder
        foreach (MapiMessage message in folder.EnumerateMapiMessages())
        {
            if (extractedCount >= maxMessages)
                return;

            // Build a safe file name based on the message subject
            string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
            string safeFileName = GetSafeFileName(subject) + ".msg";
            string filePath = Path.Combine(outputDir, safeFileName);

            // Save the message to a .msg file
            try
            {
                message.Save(filePath);
                extractedCount++;
                Console.WriteLine($"Saved: {filePath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save message '{subject}': {saveEx.Message}");
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            if (extractedCount >= maxMessages)
                return;

            ProcessFolder(subFolder, outputDir, ref extractedCount, maxMessages);
        }
    }

    // Replaces invalid filename characters with an underscore
    private static string GetSafeFileName(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }
        return name;
    }
}
