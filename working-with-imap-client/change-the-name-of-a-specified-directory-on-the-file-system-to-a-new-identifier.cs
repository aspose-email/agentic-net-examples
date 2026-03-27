using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Define the original directory and the new name.
            string sourcePath = @"C:\Temp\OldFolder";
            string destinationPath = @"C:\Temp\NewFolder";

            // Verify that the source directory exists.
            if (!Directory.Exists(sourcePath))
            {
                Console.Error.WriteLine("Source directory does not exist: " + sourcePath);
                return;
            }

            // Verify that the destination directory does not already exist.
            if (Directory.Exists(destinationPath))
            {
                Console.Error.WriteLine("Destination directory already exists: " + destinationPath);
                return;
            }

            // Ensure the parent directory for the destination exists.
            string parentDirectory = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }

            // Rename (move) the directory.
            Directory.Move(sourcePath, destinationPath);
            Console.WriteLine("Directory renamed successfully to: " + destinationPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred: " + ex.Message);
        }
    }
}