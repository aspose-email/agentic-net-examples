using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailMessageExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "storage.pst";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Input file not found: {pstPath}");
                    return;
                }

                // Define output directory and ensure it exists
                string outputDir = "ExtractedMessages";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                int totalConverted = 0;
                int totalSkipped = 0;
                List<string> errorMessages = new List<string>();

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through each subfolder of the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            try
                            {
                                // Extract the full message object as MapiMessage
                                MapiMessage msg = pst.ExtractMessage(messageInfo);

                                // Build a safe filename using the subject
                                string safeSubject = SanitizeFileName(msg.Subject);
                                if (string.IsNullOrWhiteSpace(safeSubject))
                                {
                                    safeSubject = "Untitled";
                                }

                                // Ensure unique filename
                                string fileName = $"{safeSubject}.msg";
                                string outputPath = Path.Combine(outputDir, fileName);
                                int duplicateIndex = 1;
                                while (File.Exists(outputPath))
                                {
                                    fileName = $"{safeSubject}_{duplicateIndex}.msg";
                                    outputPath = Path.Combine(outputDir, fileName);
                                    duplicateIndex++;
                                }

                                // Save the message as a .msg file
                                msg.Save(outputPath);
                                totalConverted++;
                            }
                            catch (Exception ex)
                            {
                                // Record any errors for this message and continue
                                totalSkipped++;
                                errorMessages.Add($"Failed to process message '{messageInfo.Subject}': {ex.Message}");
                                Console.Error.WriteLine($"Error: {ex.Message}");
                            }
                        }
                    }
                }

                // Summary report
                Console.WriteLine();
                Console.WriteLine("=== Extraction Summary ===");
                Console.WriteLine($"Total messages converted: {totalConverted}");
                Console.WriteLine($"Total messages skipped:   {totalSkipped}");
                if (errorMessages.Count > 0)
                {
                    Console.WriteLine("Errors encountered:");
                    foreach (string err in errorMessages)
                    {
                        Console.WriteLine($"- {err}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Helper to replace invalid filename characters
        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
    }
}
