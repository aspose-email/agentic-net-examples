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
            // Path to the PST/OST file
            string pstPath = "storage.pst";

            // Verify the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Output directory for restored messages
            string outputDir = "SoftDeletedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve soft‑deleted items
                IList<RestoredItemEntry> restoredItems = pst.FindAndExtractSoftDeletedItems();

                Console.WriteLine($"Found {restoredItems.Count} soft‑deleted items.");

                foreach (RestoredItemEntry entry in restoredItems)
                {
                    // The restored item contains the original MAPI message
                    MapiMessage mapiMsg = entry.Item as MapiMessage;
                    if (mapiMsg == null)
                    {
                        continue;
                    }

                    // Create a safe filename from the subject
                    string safeSubject = string.IsNullOrEmpty(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(c, '_');
                    }

                    // Truncate if too long for file system
                    int maxFileNameLength = 200;
                    if (safeSubject.Length > maxFileNameLength)
                    {
                        safeSubject = safeSubject.Substring(0, maxFileNameLength);
                    }

                    string filePath = Path.Combine(outputDir, $"{safeSubject}.msg");

                    try
                    {
                        // Save the message as a .msg file
                        mapiMsg.Save(filePath);
                        Console.WriteLine($"Saved: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message '{safeSubject}': {ex.Message}");
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
