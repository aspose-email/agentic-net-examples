using System;
using System.IO;

namespace DeleteDirectorySample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top-level exception guard
            try
            {
                // Specify the directory to delete
                string directoryPath = "C:\\Temp\\MyFolder";

                // Guard: ensure the directory exists before attempting deletion
                if (!Directory.Exists(directoryPath))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                    return;
                }

                // Attempt to delete the directory and all its contents
                try
                {
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine($"Directory deleted: {directoryPath}");
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during deletion
                    Console.Error.WriteLine($"Error deleting directory: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors in the program
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
