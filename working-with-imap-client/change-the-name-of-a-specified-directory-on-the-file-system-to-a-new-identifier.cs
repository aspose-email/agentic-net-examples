using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the source (existing) directory and the new name.
            string sourceDirectory = "OldDirectory";
            string targetDirectory = "NewDirectory";

            // Ensure the source directory exists; create a placeholder if it does not.
            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
                Console.WriteLine($"Created placeholder directory: {sourceDirectory}");
            }

            // Check that the target directory does not already exist to avoid exceptions.
            if (Directory.Exists(targetDirectory))
            {
                Console.Error.WriteLine($"Target directory already exists: {targetDirectory}");
                return;
            }

            // Rename (move) the directory.
            Directory.Move(sourceDirectory, targetDirectory);
            Console.WriteLine($"Directory renamed from '{sourceDirectory}' to '{targetDirectory}'.");
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application.
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
