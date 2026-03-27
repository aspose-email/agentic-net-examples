using System;
using System.IO;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top-level exception guard
            try
            {
                // Specify the directory to delete
                string directoryPath = @"C:\Temp\MyFolder";

                // Guard against missing directory
                if (!Directory.Exists(directoryPath))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                    return;
                }

                // Attempt to delete the directory and its contents
                try
                {
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine($"Directory deleted successfully: {directoryPath}");
                }
                catch (IOException ioEx)
                {
                    Console.Error.WriteLine($"IO error while deleting directory: {ioEx.Message}");
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    Console.Error.WriteLine($"Access denied while deleting directory: {uaEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error while deleting directory: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors in the program
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
            }
        }
    }
}
