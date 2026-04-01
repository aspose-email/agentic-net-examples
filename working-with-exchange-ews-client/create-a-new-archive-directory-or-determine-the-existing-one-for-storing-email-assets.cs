using System;
using System.IO;
using Aspose.Email;

namespace EmailArchiveSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Determine base directory of the application
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Define the archive folder path
                string archiveFolderPath = Path.Combine(baseDirectory, "EmailArchive");

                // Guarded file system operation: check existence and create if necessary
                if (!Directory.Exists(archiveFolderPath))
                {
                    DirectoryInfo archiveDirectory = Directory.CreateDirectory(archiveFolderPath);
                    Console.WriteLine($"Created archive directory at: {archiveDirectory.FullName}");
                }
                else
                {
                    Console.WriteLine($"Archive directory already exists at: {archiveFolderPath}");
                }
            }
            catch (Exception exception)
            {
                // Top‑level exception handling
                Console.Error.WriteLine($"An error occurred: {exception.Message}");
            }
        }
    }
}
