using System;
using System.IO;

// Author: Example code for deleting a directory safely

class Program
{
    static void Main()
    {
        try
        {
            // Path of the directory to be removed
            string folderPath = @"C:\Temp\DeleteMe";

            // Verify that the directory exists before attempting deletion
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Directory does not exist: {folderPath}");
                return;
            }

            try
            {
                // Delete the directory and all its contents permanently
                Directory.Delete(folderPath, recursive: true);
                Console.WriteLine($"Directory deleted: {folderPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete directory: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
