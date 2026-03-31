using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Specify the directory path to be created
            string directoryPath = @"C:\Temp\AsposeDemo";

            // Guard the file system operation
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    Console.WriteLine($"Directory created: {directoryPath}");
                }
                else
                {
                    Console.WriteLine($"Directory already exists: {directoryPath}");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error creating directory – {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
