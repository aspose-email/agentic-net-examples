using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source PST file
            string pstFilePath = "input.pst";

            // Folder where split PST parts will be saved
            string outputFolderPath = "output_parts";

            // Verify source PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstFilePath}");
                return;
            }

            // Ensure the output folder exists
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            // Open the source PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Define the approximate size of each split part (e.g., 10 MB)
                long chunkSizeInBytes = 10L * 1024L * 1024L; // 10 MB

                // Split the PST into parts asynchronously and wait for completion
                Task splitTask = pst.SplitIntoAsync(chunkSizeInBytes, outputFolderPath);
                splitTask.Wait();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
