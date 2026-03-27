using System;
using System.IO;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string directoryPath = args.Length > 0 ? args[0] : @"C:\Temp\MyFolder";

                if (!Directory.Exists(directoryPath))
                {
                    Console.Error.WriteLine($"Error: Directory does not exist – {directoryPath}");
                    return;
                }

                try
                {
                    // Delete the directory and all its contents permanently
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine($"Directory deleted successfully: {directoryPath}");
                }
                catch (Exception deleteEx)
                {
                    Console.Error.WriteLine($"Error deleting directory – {deleteEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error – {ex.Message}");
                return;
            }
        }
    }
}