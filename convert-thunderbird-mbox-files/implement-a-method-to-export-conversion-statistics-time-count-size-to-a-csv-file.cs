using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "TestEml.eml";
            string outputPath = "output.msg";
            string csvPath = "conversion_stats.csv";

            // Ensure the input EML file exists; create a minimal placeholder if missing
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

                string minimalEml = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nHello world.";
                try
                {
                    File.WriteAllText(inputPath, minimalEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the EML file with load options
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            DateTime start = DateTime.UtcNow;
            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                // Convert and save as MSG
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }
            DateTime end = DateTime.UtcNow;
            TimeSpan conversionDuration = end - start;

            // Determine the size of the generated MSG file
            long outputSize = 0;
            if (File.Exists(outputPath))
            {
                try
                {
                    FileInfo info = new FileInfo(outputPath);
                    outputSize = info.Length;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to obtain output file size: {ex.Message}");
                }
            }

            // Export conversion statistics to CSV
            ExportStatistics(csvPath, conversionDuration, 1, outputSize);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Writes conversion statistics (time, count, size) to a CSV file.
    // Author: Aspose.Email example
    private static void ExportStatistics(string csvFilePath, TimeSpan conversionTime, int messageCount, long totalSizeBytes)
    {
        try
        {
            string directory = Path.GetDirectoryName(csvFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                // CSV header
                writer.WriteLine("ConversionTimeSeconds,MessageCount,TotalSizeBytes");
                // CSV data row
                writer.WriteLine($"{conversionTime.TotalSeconds},{messageCount},{totalSizeBytes}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
        }
    }
}
