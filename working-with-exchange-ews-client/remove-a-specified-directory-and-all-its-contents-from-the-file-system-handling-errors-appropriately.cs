using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Specify the directory to remove
            string folderPath = @"C:\Path\To\Delete";

            // Verify that the directory exists before attempting deletion
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Directory does not exist: {folderPath}");
                return;
            }

            try
            {
                // Delete the directory and all its contents
                Directory.Delete(folderPath, true);
                Console.WriteLine($"Deleted directory: {folderPath}");
            }
            catch (Exception deleteEx)
            {
                Console.Error.WriteLine($"Failed to delete directory: {deleteEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
