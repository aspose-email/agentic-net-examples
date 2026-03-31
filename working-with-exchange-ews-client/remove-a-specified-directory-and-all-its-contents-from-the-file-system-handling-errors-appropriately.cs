using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Specify the directory to delete
            string directoryPath = @"C:\Temp\MyFolder";

            // Guard: ensure the directory exists before attempting deletion
            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Directory does not exist: {directoryPath}");
                return;
            }

            try
            {
                // Delete the directory and all its contents
                Directory.Delete(directoryPath, true);
                Console.WriteLine($"Successfully deleted directory: {directoryPath}");
            }
            catch (Exception deleteEx)
            {
                Console.Error.WriteLine($"Error deleting directory: {deleteEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
