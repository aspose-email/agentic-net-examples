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
                string basePath = Directory.GetCurrentDirectory();
                string newFolderName = "NewFolder";
                string newDirectoryPath = Path.Combine(basePath, newFolderName);

                // Check if the directory already exists
                if (Directory.Exists(newDirectoryPath))
                {
                    Console.WriteLine($"Directory already exists: {newDirectoryPath}");
                }
                else
                {
                    // Attempt to create the directory with error handling
                    try
                    {
                        DirectoryInfo createdDir = Directory.CreateDirectory(newDirectoryPath);
                        Console.WriteLine($"Directory created successfully: {createdDir.FullName}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{newDirectoryPath}': {ioEx.Message}");
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