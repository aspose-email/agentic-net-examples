using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine whether compression of split parts is enabled via command‑line switch.
            bool enableCompression = false;
            foreach (string arg in args)
            {
                if (arg.Equals("--compress", StringComparison.OrdinalIgnoreCase))
                {
                    enableCompression = true;
                    break;
                }
            }

            // Paths for the source PST and the folder where split parts will be placed.
            const string pstPath = "sample.pst";
            const string outputFolder = "SplitParts";

            // Ensure the output folder exists.
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                return;
            }

            // Ensure a PST file exists; create a minimal placeholder if missing.
            try
            {
                if (!File.Exists(pstPath))
                {
                    // Create an empty PST with Unicode format.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder PST '{pstPath}': {ex.Message}");
                return;
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Define a chunk size (e.g., 10 MB).
                    long chunkSize = 10 * 1024 * 1024;

                    // If compression is requested, we could adjust processing here.
                    // The SplitInto method does not expose a compression flag directly,
                    // but this placeholder demonstrates where such logic would be applied.
                    if (enableCompression)
                    {
                        Console.WriteLine("Compression of split parts is enabled (placeholder logic).");
                        // Insert any additional steps needed for compression here.
                    }
                    else
                    {
                        Console.WriteLine("Compression of split parts is disabled.");
                    }

                    // Perform the split operation.
                    pst.SplitInto(chunkSize, outputFolder);
                    Console.WriteLine($"PST split completed. Parts are stored in '{outputFolder}'.");
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
