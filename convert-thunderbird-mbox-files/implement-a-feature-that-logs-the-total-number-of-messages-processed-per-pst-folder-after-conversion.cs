using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "storage.pst";

                // Verify PST file exists before attempting to load
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Base output directory for extracted messages
                string baseOutputDir = "ExtractedMessages";

                // Ensure the base output directory exists
                Directory.CreateDirectory(baseOutputDir);

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    int totalItemsCount = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items count: {totalItemsCount}");

                    // Iterate through each subfolder in the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Create a subdirectory for this folder's messages
                        string folderOutputDir = Path.Combine(baseOutputDir, MakeSafeFileName(folderInfo.DisplayName));
                        Directory.CreateDirectory(folderOutputDir);

                        int processedCount = 0;
                        int duplicateIndex = 0;

                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            try
                            {
                                // Extract the full message as a MapiMessage
                                MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                                // Sanitize subject for use as a filename
                                string safeSubject = string.IsNullOrEmpty(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                                safeSubject = MakeSafeFileName(safeSubject);

                                // Ensure unique filename within the folder
                                string outputFileName = safeSubject + ".msg";
                                string outputPath = Path.Combine(folderOutputDir, outputFileName);
                                while (File.Exists(outputPath))
                                {
                                    duplicateIndex++;
                                    outputFileName = $"{safeSubject}_{duplicateIndex}.msg";
                                    outputPath = Path.Combine(folderOutputDir, outputFileName);
                                }

                                // Save the message as a .msg file
                                mapiMsg.Save(outputPath);
                                processedCount++;
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to process a message in folder '{folderInfo.DisplayName}': {ex.Message}");
                            }
                        }

                        // Log the number of processed messages for this folder
                        Console.WriteLine($"Processed messages in folder '{folderInfo.DisplayName}': {processedCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper method to replace invalid filename characters
        private static string MakeSafeFileName(string name)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar, '_');
            }
            return name;
        }
    }
}
