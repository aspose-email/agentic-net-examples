using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";
            string outputDirectory = "ExtractedMessages";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required for an empty PST
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through each subfolder in the PST root
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName}");
                        Console.WriteLine($"Total items: {folder.ContentCount}");
                        Console.WriteLine($"Unread items: {folder.ContentUnreadCount}");

                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            // Extract the full message
                            using (MapiMessage message = pst.ExtractMessage(messageInfo))
                            {
                                // Sanitize subject to create a valid file name
                                string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(invalidChar, '_');
                                }

                                string outputPath = Path.Combine(outputDirectory, $"{safeSubject}.msg");

                                try
                                {
                                    message.Save(outputPath);
                                    Console.WriteLine($"Saved: {outputPath}");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Error saving message '{message.Subject}': {ex.Message}");
                                }
                            }
                        }
                    }
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
}
