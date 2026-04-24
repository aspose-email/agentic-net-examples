using Aspose.Email;
using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email.Storage.Pst;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Paths for the source PST file and the destination directory
            string inputPstPath = "input.pst";
            string outputDirectory = "output";

            // Ensure the destination directory exists; create it if necessary
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Ensure the source PST file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                // Create a new empty PST file with Unicode format
                PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Define the approximate chunk size (e.g., 10 MB)
                long chunkSize = 10L * 1024L * 1024L;

                // Split the PST into smaller parts in the specified output directory
                await pst.SplitIntoAsync(chunkSize, outputDirectory);
            }

            Console.WriteLine("PST splitting completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
