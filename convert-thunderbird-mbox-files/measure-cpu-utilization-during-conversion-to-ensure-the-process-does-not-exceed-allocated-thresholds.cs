using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email;

namespace EmailConversionSample
{
    // Author: Generated example for EML to MSG conversion with CPU utilization monitoring
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "TestEml.eml";
                string outputPath = "output.msg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Start CPU usage measurement
                Process currentProcess = Process.GetCurrentProcess();
                TimeSpan cpuStart = currentProcess.TotalProcessorTime;
                DateTime wallStart = DateTime.UtcNow;

                // Initialize load options for EML
                EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                // Load the EML file and convert to MSG
                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }

                // End CPU usage measurement
                currentProcess.Refresh();
                TimeSpan cpuEnd = currentProcess.TotalProcessorTime;
                TimeSpan cpuUsed = cpuEnd - cpuStart;
                TimeSpan wallElapsed = DateTime.UtcNow - wallStart;

                Console.WriteLine($"CPU time used: {cpuUsed.TotalMilliseconds} ms");
                Console.WriteLine($"Wall time elapsed: {wallElapsed.TotalMilliseconds} ms");

                // Example threshold check (adjust as needed)
                double cpuThresholdMs = 500.0;
                if (cpuUsed.TotalMilliseconds > cpuThresholdMs)
                {
                    Console.Error.WriteLine($"CPU utilization exceeded threshold of {cpuThresholdMs} ms.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
