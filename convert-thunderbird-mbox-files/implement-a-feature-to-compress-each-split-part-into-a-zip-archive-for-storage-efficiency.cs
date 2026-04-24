using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string pstFilePath = "storage.pst";
            string splitOutputFolder = "SplitParts";
            string partFileNamePrefix = "Archive";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output folder exists
            try
            {
                if (!Directory.Exists(splitOutputFolder))
                {
                    Directory.CreateDirectory(splitOutputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                return;
            }

            // Split the PST into parts (approx. 10 MB each)
            long chunkSize = 10L * 1024 * 1024; // 10 MB

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // SplitInto creates files named {prefix}_part{number}.pst in the specified folder
                    pst.SplitInto(chunkSize, splitOutputFolder, partFileNamePrefix);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during PST splitting: {ex.Message}");
                return;
            }

            // Compress each generated PST part into a ZIP archive
            string[] pstPartFiles;
            try
            {
                pstPartFiles = Directory.GetFiles(splitOutputFolder, "*.pst");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate split PST files: {ex.Message}");
                return;
            }

            foreach (string pstPartPath in pstPartFiles)
            {
                string zipFilePath = Path.ChangeExtension(pstPartPath, ".zip");

                try
                {
                    using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                        {
                            string entryName = Path.GetFileName(pstPartPath);
                            archive.CreateEntryFromFile(pstPartPath, entryName);
                        }
                    }

                    Console.WriteLine($"Compressed '{pstPartPath}' to '{zipFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to compress '{pstPartPath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
