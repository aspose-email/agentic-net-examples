using System;
using System.IO;

namespace AsposeEmailDirectoryRemoval
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Verify that a directory path was provided.
                if (args == null || args.Length == 0)
                {
                    Console.Error.WriteLine("Error: No directory path specified.");
                    return;
                }

                string directoryPath = args[0];

                // Guard against missing directory.
                if (!Directory.Exists(directoryPath))
                {
                    Console.Error.WriteLine($"Error: Directory does not exist – {directoryPath}");
                    return;
                }

                try
                {
                    // Delete the directory and all its contents.
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine($"Directory successfully removed: {directoryPath}");
                }
                catch (Exception ioEx)
                {
                    // Handle any I/O errors that occur during deletion.
                    Console.Error.WriteLine($"Error deleting directory '{directoryPath}': {ioEx.Message}");
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}