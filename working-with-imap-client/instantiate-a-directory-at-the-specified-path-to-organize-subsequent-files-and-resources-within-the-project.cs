using System;
using System.IO;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Specify the directory path to be created or ensured.
                string directoryPath = "C:\\Temp\\AsposeSample";

                // Verify that the directory exists; if not, attempt to create it.
                if (!Directory.Exists(directoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Could not create directory – {directoryPath}. {ex.Message}");
                        return;
                    }
                }

                Console.WriteLine($"Directory ensured at: {directoryPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled error: {ex.Message}");
                return;
            }
        }
    }
}