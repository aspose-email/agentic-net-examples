using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Path of the directory to be renamed
            string sourceDirectory = @"C:\Temp\OldFolder";

            // New name for the directory (without path)
            string newDirectoryName = "NewFolder";

            // Verify that the source directory exists; create a placeholder if it does not
            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
                Console.WriteLine($"Source directory was missing. Created placeholder at '{sourceDirectory}'.");
            }

            // Determine the target directory path
            string parentPath = Path.GetDirectoryName(sourceDirectory);
            string targetDirectory = Path.Combine(parentPath, newDirectoryName);

            // Ensure the target directory does not already exist
            if (Directory.Exists(targetDirectory))
            {
                Console.Error.WriteLine($"Target directory '{targetDirectory}' already exists. Rename operation aborted.");
                return;
            }

            // Perform the rename operation
            Directory.Move(sourceDirectory, targetDirectory);
            Console.WriteLine($"Directory successfully renamed to '{targetDirectory}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
