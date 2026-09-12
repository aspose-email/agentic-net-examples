using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author: Example to measure disk I/O throughput during EML to MSG conversion.

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare load options
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            // Measure load time
            Stopwatch loadTimer = Stopwatch.StartNew();
            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                loadTimer.Stop();

                FileInfo inputInfo = new FileInfo(inputPath);
                long inputBytes = inputInfo.Length;
                double loadSeconds = loadTimer.Elapsed.TotalSeconds;
                double readThroughput = loadSeconds > 0 ? inputBytes / loadSeconds : 0;

                Console.WriteLine($"Loaded {inputBytes} bytes in {loadSeconds:F3}s (Read throughput: {readThroughput / 1024 / 1024:F2} MB/s)");

                // Measure save time
                Stopwatch saveTimer = Stopwatch.StartNew();
                message.Save(outputPath, SaveOptions.DefaultMsg);
                saveTimer.Stop();

                FileInfo outputInfo = new FileInfo(outputPath);
                long outputBytes = outputInfo.Length;
                double saveSeconds = saveTimer.Elapsed.TotalSeconds;
                double writeThroughput = saveSeconds > 0 ? outputBytes / saveSeconds : 0;

                Console.WriteLine($"Saved {outputBytes} bytes in {saveSeconds:F3}s (Write throughput: {writeThroughput / 1024 / 1024:F2} MB/s)");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
