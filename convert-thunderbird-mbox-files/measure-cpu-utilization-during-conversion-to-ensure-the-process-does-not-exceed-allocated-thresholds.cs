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
            // Define input and output file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    // Create an empty placeholder MBOX file
                    using (FileStream placeholder = File.Create(mboxFilePath))
                    {
                        // No content needed for placeholder
                    }
                    Console.WriteLine($"Placeholder MBOX file created at '{mboxFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory '{pstDirectory}': {ex.Message}");
                    return;
                }
            }

            // Measure CPU usage before conversion
            Process currentProcess = Process.GetCurrentProcess();
            TimeSpan cpuStart = currentProcess.TotalProcessorTime;
            DateTime wallStart = DateTime.UtcNow;

            // Perform the conversion inside a guarded file I/O block
            try
            {
                using (FileStream mboxStream = File.OpenRead(mboxFilePath))
                using (FileStream pstStream = File.Create(pstFilePath))
                {
                    MailStorageConverter.MboxToPst(mboxStream, pstStream);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Measure CPU usage after conversion
            TimeSpan cpuEnd = currentProcess.TotalProcessorTime;
            DateTime wallEnd = DateTime.UtcNow;

            TimeSpan cpuUsed = cpuEnd - cpuStart;
            TimeSpan wallElapsed = wallEnd - wallStart;

            // Calculate CPU utilization percentage
            double cpuUtilization = 0;
            if (wallElapsed.TotalMilliseconds > 0)
            {
                cpuUtilization = (cpuUsed.TotalMilliseconds / (Environment.ProcessorCount * wallElapsed.TotalMilliseconds)) * 100.0;
            }

            Console.WriteLine($"Conversion completed. CPU utilization: {cpuUtilization:F2}% over {wallElapsed.TotalSeconds:F2} seconds.");

            // Threshold check (example: 80%)
            const double cpuThreshold = 80.0;
            if (cpuUtilization > cpuThreshold)
            {
                Console.WriteLine($"Warning: CPU utilization exceeded the threshold of {cpuThreshold}%.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
