using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "input.pst";
            string outputDirectory = "output";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST and process messages
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                int totalItems = pst.Store.GetTotalItemsCount();
                int processedCount = 0;

                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        using (MapiMessage message = pst.ExtractMessage(messageInfo))
                        {
                            // Create a safe file name from the subject
                            string safeSubject = message.Subject ?? "NoSubject";
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(invalidChar, '_');
                            }
                            string outputPath = Path.Combine(outputDirectory, $"{safeSubject}.msg");

                            try
                            {
                                message.Save(outputPath);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{message.Subject}': {saveEx.Message}");
                                // Continue processing other messages
                            }
                        }

                        processedCount++;
                        Console.WriteLine($"Processed {processedCount}/{totalItems} messages");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
