using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source PST and the folder where split parts will be stored
            string pstPath = "sample.pst";
            string splitFolder = "SplitParts";

            // Ensure the PST file exists; if not, create a minimal placeholder PST
            if (!File.Exists(pstPath))
            {
                // Create a new empty PST file (Unicode format)
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Ensure the output folder exists
            if (!Directory.Exists(splitFolder))
            {
                Directory.CreateDirectory(splitFolder);
            }

            // Measure memory usage before splitting
            long memoryBefore = GC.GetTotalMemory(forceFullCollection: true);
            Console.WriteLine($"Memory before split: {memoryBefore / 1024} KB");

            // Open the PST file and perform the split operation
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Define an approximate chunk size (e.g., 10 MB)
                long chunkSize = 10 * 1024 * 1024; // 10 MB

                // Split the PST into smaller parts; each part will be placed in the splitFolder
                pst.SplitInto(chunkSize, splitFolder);
            }

            // Measure memory usage after splitting
            long memoryAfter = GC.GetTotalMemory(forceFullCollection: true);
            Console.WriteLine($"Memory after split: {memoryAfter / 1024} KB");

            // Report the memory difference
            long memoryDifference = memoryAfter - memoryBefore;
            Console.WriteLine($"Memory change: {memoryDifference / 1024} KB");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
