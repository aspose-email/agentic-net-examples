using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Define the original directory and the new name
            string sourceDirectory = "C:\\Temp\\OldFolder";
            string targetDirectory = "C:\\Temp\\NewFolder";

            // Verify that the source directory exists
            if (!Directory.Exists(sourceDirectory))
            {
                Console.Error.WriteLine("Source directory does not exist: " + sourceDirectory);
                return;
            }

            // Ensure the target directory does not already exist
            if (Directory.Exists(targetDirectory))
            {
                Console.Error.WriteLine("Target directory already exists: " + targetDirectory);
                return;
            }

            // Perform the rename operation inside its own try/catch to capture I/O errors
            try
            {
                Directory.Move(sourceDirectory, targetDirectory);
                Console.WriteLine("Directory renamed successfully from '{0}' to '{1}'.", sourceDirectory, targetDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to rename directory: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}