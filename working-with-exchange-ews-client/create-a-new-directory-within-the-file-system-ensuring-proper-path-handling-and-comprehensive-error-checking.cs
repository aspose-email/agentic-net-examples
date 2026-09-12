using System;
using System.IO;

namespace FileSystemExample
{
    // Author: Aspose.Email expert - example demonstrating safe directory creation with comprehensive error handling.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Determine the target directory path.
                string targetPath;
                if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
                {
                    targetPath = args[0];
                }
                else
                {
                    // Default to a folder named "NewFolder" in the current working directory.
                    targetPath = Path.Combine(Directory.GetCurrentDirectory(), "NewFolder");
                }

                // Resolve to an absolute path.
                string fullPath = Path.GetFullPath(targetPath);

                // Ensure the parent directory exists.
                string parentDirectory = Path.GetDirectoryName(fullPath);
                if (string.IsNullOrEmpty(parentDirectory))
                {
                    Console.Error.WriteLine("Invalid path specified.");
                    return;
                }

                if (!Directory.Exists(parentDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(parentDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create parent directory '{parentDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Create the target directory if it does not already exist.
                if (Directory.Exists(fullPath))
                {
                    Console.WriteLine($"Directory already exists: {fullPath}");
                }
                else
                {
                    try
                    {
                        DirectoryInfo createdInfo = Directory.CreateDirectory(fullPath);
                        Console.WriteLine($"Directory created successfully: {createdInfo.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{fullPath}': {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                // Gracefully exit without rethrowing.
            }
        }
    }
}
