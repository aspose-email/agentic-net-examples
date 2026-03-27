using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Determine the archive directory path relative to the current working directory
            string archivePath = Path.Combine(Environment.CurrentDirectory, "EmailArchive");

            // Ensure the directory exists; create it if it does not
            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
                Console.WriteLine($"Created archive directory: {archivePath}");
            }
            else
            {
                Console.WriteLine($"Archive directory already exists: {archivePath}");
            }
        }
        catch (Exception ex)
        {
            // Output any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
