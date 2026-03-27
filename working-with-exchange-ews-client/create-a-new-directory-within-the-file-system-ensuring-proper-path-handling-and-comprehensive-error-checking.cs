using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Determine the base path (current working directory)
            string basePath = Environment.CurrentDirectory;

            // Define the name of the new directory
            string newFolderName = "NewDirectory";

            // Combine paths safely
            string newFolderPath = Path.Combine(basePath, newFolderName);

            // Check if the directory already exists
            if (Directory.Exists(newFolderPath))
            {
                Console.WriteLine($"Directory already exists: {newFolderPath}");
            }
            else
            {
                try
                {
                    // Create the directory
                    DirectoryInfo createdDir = Directory.CreateDirectory(newFolderPath);
                    Console.WriteLine($"Directory created: {createdDir.FullName}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating directory: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
