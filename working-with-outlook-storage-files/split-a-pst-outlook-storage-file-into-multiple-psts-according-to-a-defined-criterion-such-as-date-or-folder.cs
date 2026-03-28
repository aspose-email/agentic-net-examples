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
            // Define the source PST file and the destination folder for split parts
            string sourcePstPath = "source.pst";
            string destinationFolderPath = "SplitParts";

            // Verify that the source PST file exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {sourcePstPath}");
                return;
            }

            // Ensure the destination folder exists
            if (!Directory.Exists(destinationFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {destinationFolderPath}. {dirEx.Message}");
                    return;
                }
            }

            // Define the approximate size of each split part (e.g., 10 MB)
            long chunkSizeInBytes = 10L * 1024L * 1024L; // 10 MB

            // Open the source PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(sourcePstPath))
            {
                // Split the PST into multiple parts based on the defined chunk size
                // The second argument is the folder where the split PST files will be created
                pst.SplitInto(chunkSizeInBytes, destinationFolderPath);
                Console.WriteLine("PST splitting completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
