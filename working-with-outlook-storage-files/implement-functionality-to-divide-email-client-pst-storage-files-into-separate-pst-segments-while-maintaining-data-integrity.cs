using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates splitting a PST file into smaller parts.
            string pstFilePath = "input.pst";
            string outputFolder = "PstParts";
            long chunkSizeBytes = 10L * 1024 * 1024; // 10 MB per part
            string partFileNamePrefix = "Part_";

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputFolder}': {ex.Message}");
                return;
            }

            // Verify the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    // Create an empty PST with Unicode format.
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST and split it.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    pst.SplitInto(chunkSizeBytes, partFileNamePrefix, outputFolder);
                    Console.WriteLine($"PST split completed. Parts are located in '{outputFolder}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during PST processing: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
