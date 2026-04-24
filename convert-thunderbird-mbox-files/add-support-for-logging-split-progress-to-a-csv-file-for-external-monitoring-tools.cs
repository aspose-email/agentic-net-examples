using Aspose.Email;
using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path
            string pstFilePath = "input.pst";
            // Output folder for split PST chunks
            string outputFolder = "output_chunks";
            // CSV log file path
            string csvLogPath = "split_progress.csv";
            // Desired chunk size (e.g., 10 MB)
            long chunkSize = 10L * 1024 * 1024;

            // Verify input PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Input PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output folder exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                return;
            }

            // Open CSV writer for logging split progress
            using (StreamWriter csvWriter = new StreamWriter(csvLogPath, append: true))
            {
                // Write CSV header if file is empty
                if (csvWriter.BaseStream.Length == 0)
                {
                    csvWriter.WriteLine("Timestamp,ChunkFileName");
                }

                // Open the PST storage
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Subscribe to the StorageProcessed event to log each chunk creation
                    pst.StorageProcessed += (sender, e) =>
                    {
                        // e.FileName contains the name of the created chunk
                        string line = $"{DateTime.UtcNow:O},{e.FileName}";
                        csvWriter.WriteLine(line);
                        csvWriter.Flush();
                    };

                    // Perform the split operation synchronously
                    try
                    {
                        Task splitTask = pst.SplitIntoAsync(chunkSize, outputFolder);
                        splitTask.GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                        return;
                    }
                }
            }

            Console.WriteLine("PST split completed. Progress logged to CSV.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
