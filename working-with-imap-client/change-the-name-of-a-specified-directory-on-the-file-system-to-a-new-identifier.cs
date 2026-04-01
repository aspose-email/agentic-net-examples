using System;
using System.IO;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define the existing directory and the new name
                string sourceDirectory = "OldDirectory";
                string targetDirectory = "NewDirectory";

                // Guard: ensure the source directory exists; create a placeholder if it does not
                if (!Directory.Exists(sourceDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(sourceDirectory);
                        Console.WriteLine($"Source directory '{sourceDirectory}' was missing and has been created as a placeholder.");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder directory: {ioEx.Message}");
                        return;
                    }
                }

                // Guard: ensure the target directory does not already exist
                if (Directory.Exists(targetDirectory))
                {
                    Console.Error.WriteLine($"Target directory '{targetDirectory}' already exists. Rename operation aborted.");
                    return;
                }

                // Perform the rename operation
                try
                {
                    Directory.Move(sourceDirectory, targetDirectory);
                    Console.WriteLine($"Directory renamed from '{sourceDirectory}' to '{targetDirectory}'.");
                }
                catch (Exception renameEx)
                {
                    Console.Error.WriteLine($"Failed to rename directory: {renameEx.Message}");
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
