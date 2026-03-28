using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Extract soft‑deleted items
                    IList<RestoredItemEntry> restoredItems = pst.FindAndExtractSoftDeletedItems();

                    foreach (RestoredItemEntry entry in restoredItems)
                    {
                        MapiMessage message = entry.Item;
                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        // Replace invalid filename characters
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string outputPath = $"{safeSubject}_{Guid.NewGuid()}.msg";

                        // Save the message as MSG; no SaveOptions needed
                        message.Save(outputPath);
                        Console.WriteLine($"Restored message saved to: {outputPath}");
                    }

                    if (restoredItems.Count == 0)
                    {
                        Console.WriteLine("No soft‑deleted items were found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
