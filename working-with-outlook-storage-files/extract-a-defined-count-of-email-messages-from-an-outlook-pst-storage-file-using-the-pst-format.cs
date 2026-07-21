using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the PST file path, output directory and the maximum number of messages to extract.
            const string pstPath = "storage.pst";
            const string outputDir = "ExtractedMessages";
            const int maxMessages = 10;

            // Verify that the PST file exists before attempting to open it.
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure the output directory exists.
            Directory.CreateDirectory(outputDir);

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                int extractedCount = 0;

                // Iterate through each subfolder of the root folder.
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Enumerate messages in the current folder.
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        // Extract the full message as a MapiMessage.
                        MapiMessage message = pst.ExtractMessage(messageInfo);

                        // Build a safe filename from the subject.
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        // Ensure unique filename if needed.
                        string fileName = $"{safeSubject}.msg";
                        string outputPath = Path.Combine(outputDir, fileName);
                        int duplicateIndex = 1;
                        while (File.Exists(outputPath))
                        {
                            fileName = $"{safeSubject}_{duplicateIndex}.msg";
                            outputPath = Path.Combine(outputDir, fileName);
                            duplicateIndex++;
                        }

                        // Save the message as a .msg file.
                        try
                        {
                            message.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message '{message.Subject}': {ex.Message}");
                        }

                        extractedCount++;
                        if (extractedCount >= maxMessages)
                        {
                            Console.WriteLine($"Reached the defined limit of {maxMessages} messages.");
                            return;
                        }
                    }
                }

                Console.WriteLine($"Extraction completed. Total messages saved: {extractedCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
