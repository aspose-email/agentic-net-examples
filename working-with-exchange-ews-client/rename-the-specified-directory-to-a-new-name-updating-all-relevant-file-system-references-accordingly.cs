using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Define the original directory path and the new directory name
            string sourceDirectoryPath = "OldDirectory";
            string destinationDirectoryPath = "NewDirectory";

            // Verify that the source directory exists
            if (!Directory.Exists(sourceDirectoryPath))
            {
                Console.Error.WriteLine($"Source directory does not exist: {sourceDirectoryPath}");
                return;
            }

            // Verify that the destination directory does not already exist
            if (Directory.Exists(destinationDirectoryPath))
            {
                Console.Error.WriteLine($"Destination directory already exists: {destinationDirectoryPath}");
                return;
            }

            try
            {
                // Rename (move) the directory
                Directory.Move(sourceDirectoryPath, destinationDirectoryPath);
                Console.WriteLine($"Directory renamed from '{sourceDirectoryPath}' to '{destinationDirectoryPath}'.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the rename operation
                Console.Error.WriteLine($"Error renaming directory: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
