using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string mboxPath = "large.mbox";
            string pstPathSimple = "simple_output.pst";
            string pstPathBatch = "batch_output.pst";

            // Ensure input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                try
                {
                    // Create a minimal placeholder MBOX file with a single empty message
                    using (FileStream placeholderStream = new FileStream(mboxPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new StreamWriter(placeholderStream))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine("From: placeholder@example.com");
                        writer.WriteLine("To: placeholder@example.com");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder message.");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string outputDirSimple = Path.GetDirectoryName(pstPathSimple);
                if (!string.IsNullOrEmpty(outputDirSimple) && !Directory.Exists(outputDirSimple))
                {
                    Directory.CreateDirectory(outputDirSimple);
                }

                string outputDirBatch = Path.GetDirectoryName(pstPathBatch);
                if (!string.IsNullOrEmpty(outputDirBatch) && !Directory.Exists(outputDirBatch))
                {
                    Directory.CreateDirectory(outputDirBatch);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directories: {ex.Message}");
                return;
            }

            // Benchmark simple conversion (no batch processing)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            long beforeSimple = GC.GetTotalMemory(true);
            Stopwatch swSimple = Stopwatch.StartNew();

            using (PersonalStorage pstSimple = MailStorageConverter.MboxToPst(mboxPath, pstPathSimple))
            {
                // No additional processing
            }

            swSimple.Stop();
            long afterSimple = GC.GetTotalMemory(true);
            long memoryUsedSimple = afterSimple - beforeSimple;

            // Benchmark batch conversion using MboxToPstConversionOptions with a MailHandler
            MboxToPstConversionOptions batchOptions = new MboxToPstConversionOptions();
            batchOptions.MessageHandler = delegate (MailMessage message)
            {
                // Example batch handler: no operation (placeholder for batch logic)
            };

            GC.Collect();
            GC.WaitForPendingFinalizers();
            long beforeBatch = GC.GetTotalMemory(true);
            Stopwatch swBatch = Stopwatch.StartNew();

            using (PersonalStorage pstBatch = MailStorageConverter.MboxToPst(mboxPath, pstPathBatch, batchOptions))
            {
                // No additional processing
            }

            swBatch.Stop();
            long afterBatch = GC.GetTotalMemory(true);
            long memoryUsedBatch = afterBatch - beforeBatch;

            // Output benchmark results
            Console.WriteLine("Simple conversion:");
            Console.WriteLine($"  Time elapsed: {swSimple.Elapsed}");
            Console.WriteLine($"  Approx. memory used: {memoryUsedSimple / 1024} KB");

            Console.WriteLine("Batch conversion:");
            Console.WriteLine($"  Time elapsed: {swBatch.Elapsed}");
            Console.WriteLine($"  Approx. memory used: {memoryUsedBatch / 1024} KB");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
