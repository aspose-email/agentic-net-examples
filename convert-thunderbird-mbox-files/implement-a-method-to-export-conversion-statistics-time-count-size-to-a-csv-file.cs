using System;
using System.IO;
using System.Collections.Generic;

namespace AsposeEmailConversionStats
{
    // Represents conversion statistics for a single operation
    public class ConversionStats
    {
        public TimeSpan Duration { get; set; }
        public int MessageCount { get; set; }
        public long TotalSizeBytes { get; set; }

        public ConversionStats(TimeSpan duration, int messageCount, long totalSizeBytes)
        {
            this.Duration = duration;
            this.MessageCount = messageCount;
            this.TotalSizeBytes = totalSizeBytes;
        }
    }

    public static class StatsExporter
    {
        // Exports a collection of ConversionStats to a CSV file
        public static void ExportToCsv(string csvFilePath, List<ConversionStats> statsList)
        {
            if (string.IsNullOrEmpty(csvFilePath))
            {
                Console.Error.WriteLine("CSV file path is null or empty.");
                return;
            }

            try
            {
                string directoryPath = Path.GetDirectoryName(csvFilePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Using StreamWriter inside a using block ensures proper disposal
                using (StreamWriter writer = new StreamWriter(csvFilePath, false))
                {
                    // Write CSV header
                    writer.WriteLine("Duration,MessageCount,TotalSizeBytes");

                    // Write each statistic record
                    foreach (ConversionStats stats in statsList)
                    {
                        string line = string.Format("{0},{1},{2}",
                            stats.Duration,
                            stats.MessageCount,
                            stats.TotalSizeBytes);
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("Statistics exported successfully to: " + csvFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error exporting statistics: " + ex.Message);
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Sample data for demonstration purposes
                List<ConversionStats> sampleStats = new List<ConversionStats>();
                sampleStats.Add(new ConversionStats(TimeSpan.FromSeconds(12.5), 150, 2048000));
                sampleStats.Add(new ConversionStats(TimeSpan.FromSeconds(8.3), 80, 1024000));
                sampleStats.Add(new ConversionStats(TimeSpan.FromSeconds(20.0), 300, 4096000));

                // Define output CSV path
                string outputCsvPath = "ConversionStats.csv";

                // Export the statistics to CSV
                StatsExporter.ExportToCsv(outputCsvPath, sampleStats);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
