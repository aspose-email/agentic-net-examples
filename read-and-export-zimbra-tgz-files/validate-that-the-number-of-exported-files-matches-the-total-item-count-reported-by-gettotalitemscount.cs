using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for PST file and export directory
            string pstPath = "sample.pst";
            string exportDir = "ExportedMessages";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure export directory exists
            try
            {
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create export directory: {ex.Message}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve total items count from the PST store
                int totalItemsCount = pst.Store.GetTotalItemsCount();

                // Counter for exported files
                int exportedCount = 0;

                // Queue for folder traversal
                Queue<FolderInfo> folders = new Queue<FolderInfo>();
                folders.Enqueue(pst.RootFolder);

                while (folders.Count > 0)
                {
                    FolderInfo currentFolder = folders.Dequeue();

                    // Enqueue subfolders
                    foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                    {
                        folders.Enqueue(subFolder);
                    }

                    // Export each message in the current folder
                    foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                    {
                        try
                        {
                            // Extract the full message as a MapiMessage
                            MapiMessage msg = pst.ExtractMessage(messageInfo);

                            // Build a safe file name using the message subject (fallback to GUID)
                            string safeSubject = string.IsNullOrEmpty(msg.Subject) ? Guid.NewGuid().ToString() : msg.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }
                            string filePath = Path.Combine(exportDir, $"{safeSubject}.msg");

                            // Save the message
                            msg.Save(filePath);
                            exportedCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to export a message: {ex.Message}");
                            // Continue with next message
                        }
                    }
                }

                // Compare counts and output result
                if (exportedCount == totalItemsCount)
                {
                    Console.WriteLine($"Success: Exported file count ({exportedCount}) matches total items count ({totalItemsCount}).");
                }
                else
                {
                    Console.WriteLine($"Mismatch: Exported file count ({exportedCount}) does not match total items count ({totalItemsCount}).");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
