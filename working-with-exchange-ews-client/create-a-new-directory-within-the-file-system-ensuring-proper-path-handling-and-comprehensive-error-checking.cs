using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the full path of the new directory.
            string newDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MyNewFolder");

            // Ensure the parent directory exists.
            string parentDirectory = Path.GetDirectoryName(newDirectoryPath);
            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }

            // Create the new directory if it does not already exist.
            if (!Directory.Exists(newDirectoryPath))
            {
                Directory.CreateDirectory(newDirectoryPath);
                Console.WriteLine($"Directory created: {newDirectoryPath}");
            }
            else
            {
                Console.WriteLine($"Directory already exists: {newDirectoryPath}");
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream and exit gracefully.
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
