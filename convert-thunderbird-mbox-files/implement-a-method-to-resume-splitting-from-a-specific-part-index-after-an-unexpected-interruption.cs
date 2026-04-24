using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the PST file and the folder where split parts will be stored.
            const string pstPath = "sample.pst";
            const string outputFolder = "SplitParts";
            const string partFilePrefix = "archive";

            // Ensure the output folder exists.
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Ensure the PST file exists; if not, create a minimal placeholder PST.
            if (!File.Exists(pstPath))
            {
                // Create an empty PST with Unicode format.
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Define the approximate size of each split part (e.g., 10 MB).
                const long chunkSize = 10 * 1024 * 1024;

                // Determine how many parts already exist to resume from the next index.
                int existingParts = 0;
                if (Directory.Exists(outputFolder))
                {
                    string[] files = Directory.GetFiles(outputFolder, $"{partFilePrefix}_part*.pst");
                    existingParts = files.Length;
                }

                // If parts already exist, inform the user that splitting will continue.
                if (existingParts > 0)
                {
                    Console.WriteLine($"{existingParts} part(s) already exist. Resuming split from the next part.");
                }

                // Perform the split. The method creates parts with incremental numbering,
                // so it will continue after the existing files.
                pst.SplitInto(chunkSize, outputFolder, partFilePrefix);
                Console.WriteLine("PST splitting completed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
