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
            // Author: Sample code for safe extraction of PST messages with invalid EntryId handling
            string pstFilePath = "sample.pst";

            // Guard file existence
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "ExtractedMessages";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open PST storage
            using (PersonalStorage pstStorage = PersonalStorage.FromFile(pstFilePath))
            {
                // Get the root folder
                FolderInfo rootFolder = pstStorage.RootFolder;

                // Enumerate all message EntryIds
                foreach (string entryId in rootFolder.EnumerateMessagesEntryId())
                {
                    try
                    {
                        // Attempt to extract the message
                        MapiMessage message = pstStorage.ExtractMessage(entryId);

                        // Build a safe file name
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}_{entryId}.msg");

                        // Save the extracted message
                        message.Save(outputPath);
                        Console.WriteLine($"Extracted: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        // Log extraction failure but continue processing other messages
                        Console.Error.WriteLine($"Failed to extract message with EntryId '{entryId}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
