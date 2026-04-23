using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using Aspose.Email.Storage.Pst;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Define PST file path and ensure it exists
            string pstPath = "sample.pst";
            if (!File.Exists(pstPath))
            {
                // Create a minimal empty PST file if it does not exist
                using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // No additional setup required for an empty PST
                }
            }

            // Define output folder for split parts and ensure it exists
            string outputFolder = "PstChunks";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Approximate chunk size (e.g., 10 MB)
                long chunkSize = 10L * 1024L * 1024L;

                // Call the asynchronous SplitInto overload and await its completion
                await pst.SplitIntoAsync(chunkSize, outputFolder, CancellationToken.None);

                Console.WriteLine("PST split operation completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
