using System;
using System.IO;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define source PST file and output folder.
                string sourcePstPath = "source.pst";
                string outputFolder = "PstParts";

                // Ensure the source PST file exists; create a minimal placeholder if missing.
                if (!File.Exists(sourcePstPath))
                {
                    try
                    {
                        // Create an empty Unicode PST file.
                        using (PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode))
                        {
                            // No additional content needed for the placeholder.
                        }
                        Console.WriteLine($"Placeholder PST created at: {sourcePstPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                        return;
                    }
                }

                // Ensure the output directory exists.
                try
                {
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }

                // Open the source PST and split it into smaller parts.
                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(sourcePstPath))
                    {
                        // Define the approximate size of each split part (e.g., 10 MB).
                        long chunkSize = 10L * 1024 * 1024; // 10 megabytes

                        // Perform the split operation. Parts will be created in the output folder.
                        pst.SplitInto(chunkSize, outputFolder);
                        Console.WriteLine($"PST split completed. Parts are stored in: {outputFolder}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST splitting: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
