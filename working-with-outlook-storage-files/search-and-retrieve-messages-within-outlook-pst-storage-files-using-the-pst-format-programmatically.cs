using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PSTMessageExtractor
{
    class Program
    {
        static void Main()
        {
            // Path to the PST file
            string pstPath = "storage.pst";

            // Directory where extracted messages will be saved
            string outputDir = "ExtractedMessages";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

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
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            try
            {
                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Display total items count
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items count: {totalItems}");

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

                            // Extract the full message as a MapiMessage
                            using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                            {
                                // Prepare a safe filename based on the subject
                                string subject = string.IsNullOrEmpty(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    subject = subject.Replace(invalidChar, '_');
                                }

                                string outputPath = Path.Combine(outputDir, $"{subject}.msg");

                                // Save the message as a .msg file
                                mapiMsg.Save(outputPath);
                                Console.WriteLine($"Saved: {outputPath}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
            }
        }
    }
}
