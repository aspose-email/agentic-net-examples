using System;
using System.IO;

// Author: Generated example for removing a directory safely
class Program
{
    static void Main()
    {
        try
        {
            // Path of the directory to be removed
            string directoryPath = @"C:\Temp\FolderToDelete";

            // Ensure the directory exists before attempting deletion
            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Directory does not exist: {directoryPath}");
                return;
            }

            // Delete the directory and all its contents, handling possible errors
            try
            {
                Directory.Delete(directoryPath, recursive: true);
                Console.WriteLine($"Successfully deleted directory: {directoryPath}");
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
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
