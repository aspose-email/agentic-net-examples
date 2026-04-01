using System;
using System.IO;

namespace AsposeEmailDirectoryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the directory path to create
                string targetDirectoryPath = @"C:\Temp\NewFolder";

                // Ensure the parent directory exists before attempting to create the target
                string parentDirectoryPath = Path.GetDirectoryName(targetDirectoryPath);
                if (parentDirectoryPath == null)
                {
                    Console.Error.WriteLine("Invalid directory path.");
                    return;
                }

                if (!Directory.Exists(parentDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(parentDirectoryPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create parent directory '{parentDirectoryPath}': {ex.Message}");
                        return;
                    }
                }

                // Check if the target directory already exists
                if (Directory.Exists(targetDirectoryPath))
                {
                    Console.WriteLine($"Directory already exists: {targetDirectoryPath}");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                        Console.WriteLine($"Directory created successfully: {targetDirectoryPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{targetDirectoryPath}': {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
