using System;
using System.IO;
using Aspose.Email; // Core Aspose.Email namespace

class Program
{
    static void Main()
    {
        try
        {
            // Determine the path for the archive directory relative to the current working directory
            string archivePath = Path.Combine(Environment.CurrentDirectory, "EmailArchive");

            // Guarded file system operation: check existence before creation
            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
                Console.WriteLine($"Archive directory created at: {archivePath}");
            }
            else
            {
                Console.WriteLine($"Archive directory already exists at: {archivePath}");
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
