using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PSTIntegrityCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file
                string pstPath = "storage.pst";

                // Verify that the PST file exists before attempting to open it
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Directory where extracted messages will be saved
                string outputDir = "ExtractedMessages";
                Directory.CreateDirectory(outputDir);

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Retrieve and display the total number of items in the root folder
                    int totalItemsCount = pst.RootFolder.ContentCount;
                    Console.WriteLine($"Total items in root folder: {totalItemsCount}");

                    // Iterate through each subfolder of the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages within the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            // Extract the full message as a MapiMessage
                            using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                            {
                                // Convert to MailMessage for easier handling
                                MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                                // Prepare a safe filename based on the message subject
                                string subject = string.IsNullOrWhiteSpace(mailMsg.Subject) ? "NoSubject" : mailMsg.Subject;
                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    subject = subject.Replace(invalidChar, '_');
                                }

                                // Truncate if filename is too long
                                const int maxFileNameLength = 200;
                                if (subject.Length > maxFileNameLength)
                                {
                                    subject = subject.Substring(0, maxFileNameLength);
                                }

                                // Ensure unique filename
                                string outputFile;
                                int duplicateIndex = 0;
                                do
                                {
                                    string fileName = duplicateIndex == 0
                                        ? $"{subject}.eml"
                                        : $"{subject}_{duplicateIndex}.eml";
                                    outputFile = Path.Combine(outputDir, fileName);
                                    duplicateIndex++;
                                } while (File.Exists(outputFile));

                                // Save the message as an .eml file
                                mailMsg.Save(outputFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
