using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine the directory to delete (first argument or a placeholder path)
            string directoryPath = args.Length > 0 ? args[0] : @"C:\Temp\SampleDirectory";

            // Guard: ensure the directory exists before attempting deletion
            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                return;
            }

            try
            {
                // Delete the directory and all its contents permanently
                Directory.Delete(directoryPath, recursive: true);
                Console.WriteLine($"Directory deleted successfully: {directoryPath}");
            }
            catch (Exception ex)
            {
                // Handle any I/O errors (e.g., access denied, in use)
                Console.Error.WriteLine($"Error deleting directory: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
}
