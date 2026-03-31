using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define source PST files to merge
            string[] sourceFiles = new string[]
            {
                "source1.pst",
                "source2.pst"
            };

            // Define the output combined PST file
            string outputFile = "combined.pst";

            // Verify that each source file exists
            foreach (string sourcePath in sourceFiles)
            {
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Error: Source file not found – {sourcePath}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
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

            // Create the combined PST file and merge sources
            try
            {
                using (PersonalStorage combinedPst = PersonalStorage.Create(outputFile, FileFormatVersion.Unicode))
                {
                    try
                    {
                        combinedPst.MergeWith(sourceFiles);
                        Console.WriteLine("PST files merged successfully into " + outputFile);
                    }
                    catch (Exception mergeEx)
                    {
                        Console.Error.WriteLine($"Error during merge operation: {mergeEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception createEx)
            {
                Console.Error.WriteLine($"Error creating combined PST file: {createEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
