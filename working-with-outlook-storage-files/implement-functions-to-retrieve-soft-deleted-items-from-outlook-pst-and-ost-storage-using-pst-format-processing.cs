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
                string outputPath = "SoftDeletedItems.txt";

                // Ensure the PST file exists; create a minimal placeholder if missing
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created; no additional content required
                    }
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Enumerate soft‑deleted items
                    IEnumerable<RestoredItemEntry> softDeletedItems = pst.FindAndEnumerateSoftDeletedItems();

                    // Write details to console and to a text file
                    using (StreamWriter writer = new StreamWriter(outputPath, false))
                    {
                        foreach (RestoredItemEntry entry in softDeletedItems)
                        {
                            MapiMessage message = entry.Item as MapiMessage;
                            string subject = message != null ? message.Subject : "N/A";
                            string line = $"FolderId: {entry.FolderId}, Subject: {subject}";
                            Console.WriteLine(line);
                            writer.WriteLine(line);
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
}
