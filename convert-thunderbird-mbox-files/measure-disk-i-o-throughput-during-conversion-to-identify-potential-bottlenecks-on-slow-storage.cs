using Aspose.Email;
using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input MBOX and output PST
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if it does not
            if (!File.Exists(mboxPath))
            {
                try
                {
                    string placeholderMessage = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n";
                    using (FileStream fs = new FileStream(mboxPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(placeholderMessage);
                    }
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST directory: {ex.Message}");
                return;
            }

            // Measure file sizes before conversion
            long mboxSizeBytes;
            try
            {
                FileInfo mboxInfo = new FileInfo(mboxPath);
                mboxSizeBytes = mboxInfo.Length;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to get size of MBOX file: {ex.Message}");
                return;
            }

            // Perform the conversion while timing it
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                // Convert MBOX to PST
                MailStorageConverter.MboxToPst(mboxPath, pstPath);
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Measure PST size after conversion
            long pstSizeBytes;
            try
            {
                FileInfo pstInfo = new FileInfo(pstPath);
                pstSizeBytes = pstInfo.Length;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to get size of PST file: {ex.Message}");
                return;
            }

            // Calculate throughput (bytes per second)
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            if (elapsedSeconds <= 0)
            {
                elapsedSeconds = 0.001; // avoid division by zero
            }
            double totalBytes = mboxSizeBytes + pstSizeBytes;
            double throughput = totalBytes / elapsedSeconds;

            Console.WriteLine($"Conversion completed in {elapsedSeconds:F2} seconds.");
            Console.WriteLine($"MBOX size: {mboxSizeBytes} bytes, PST size: {pstSizeBytes} bytes.");
            Console.WriteLine($"Overall I/O throughput: {throughput:F2} bytes/second.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
