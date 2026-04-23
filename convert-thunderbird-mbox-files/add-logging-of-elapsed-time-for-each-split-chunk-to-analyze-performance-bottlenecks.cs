using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input MBOX, intermediate PST and split output folder
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";
            string splitFolder = "SplitChunks";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (var writer = new StreamWriter(mboxPath))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder MBOX message.");
                    }
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the split output directory exists
            try
            {
                if (!Directory.Exists(splitFolder))
                {
                    Directory.CreateDirectory(splitFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create split folder '{splitFolder}': {ex.Message}");
                return;
            }

            // Convert MBOX to PST
            try
            {
                // MailStorageConverter resides in Aspose.Email.Storage namespace
                MailStorageConverter.MboxToPst(mboxPath, pstPath);
                Console.WriteLine($"Converted MBOX to PST: '{pstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            // Open the PST and split it into chunks while logging elapsed time per chunk
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Stopwatch to measure each chunk processing time
                    Stopwatch chunkTimer = new Stopwatch();

                    // Event raised before a chunk is processed
                    pst.StorageProcessing += (sender, args) =>
                    {
                        chunkTimer.Restart();
                        Console.WriteLine($"Starting processing of a new PST chunk...");
                    };

                    // Event raised after a chunk has been created
                    pst.StorageProcessed += (sender, args) =>
                    {
                        chunkTimer.Stop();
                        Console.WriteLine($"Finished processing chunk. Elapsed time: {chunkTimer.ElapsedMilliseconds} ms");
                    };

                    // Define approximate chunk size (e.g., 10 MB)
                    long chunkSize = 10L * 1024 * 1024;

                    // Perform the split operation synchronously
                    pst.SplitIntoAsync(chunkSize, splitFolder).GetAwaiter().GetResult();

                    Console.WriteLine($"PST split completed. Chunks are stored in '{splitFolder}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"PST splitting failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
