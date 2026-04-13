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
            // Define PST file path
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new empty PST file (Unicode format)
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Open the PST file and process its contents
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through each subfolder in the root folder
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            // Extract the full message object
                            using (MapiMessage message = pst.ExtractMessage(messageInfo))
                            {
                                // Build a safe file name for the message
                                string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(invalidChar, '_');
                                }

                                string msgFilePath = Path.Combine(outputDir, $"{safeSubject}.msg");

                                // Save the message to a .msg file
                                try
                                {
                                    message.Save(msgFilePath);
                                    Console.WriteLine($"Saved message to '{msgFilePath}'.");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message '{message.Subject}': {saveEx.Message}");
                                }
                            } // MapiMessage disposed here
                        }
                    }
                } // PersonalStorage disposed here
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
