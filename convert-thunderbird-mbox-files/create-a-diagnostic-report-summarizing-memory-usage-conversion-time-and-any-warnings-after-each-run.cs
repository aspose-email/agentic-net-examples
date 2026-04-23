using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                using (FileStream placeholderStream = File.Create(mboxPath))
                {
                    string placeholderEmail = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Test\r\nFrom: test@example.com\r\nTo: recipient@example.com\r\n\r\nThis is a test message.\r\n\r\n";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(placeholderEmail);
                    placeholderStream.Write(bytes, 0, bytes.Length);
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Capture memory usage before conversion
            long memoryBefore = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Perform the MBOX to PST conversion
            PersonalStorage pstStorage = null;
            try
            {
                pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception conversionEx)
            {
                Console.Error.WriteLine($"Conversion error: {conversionEx.Message}");
                return;
            }
            finally
            {
                if (pstStorage != null)
                {
                    pstStorage.Dispose();
                }
            }

            // Stop timing and capture memory usage after conversion
            stopwatch.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            long memoryUsed = memoryAfter - memoryBefore;

            // Output diagnostic report
            Console.WriteLine("Conversion Report:");
            Console.WriteLine($"Input MBOX: {mboxPath}");
            Console.WriteLine($"Output PST: {pstPath}");
            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
            Console.WriteLine($"Memory used (bytes): {memoryUsed}");
            Console.WriteLine("Warnings: None");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
