using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input PST file path
            string pstFilePath = "input.pst";

            // Output directory where split PST parts will be created
            string outputDirectory = "PstParts";

            // Desired maximum size of each PST part (e.g., 10 MB)
            long chunkSizeInBytes = 10L * 1024L * 1024L;

            // Verify that the source PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstFilePath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the PST file and split it into smaller parts
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    pst.SplitInto(chunkSizeInBytes, outputDirectory);
                }

                Console.WriteLine("PST file has been successfully split into parts.");
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
