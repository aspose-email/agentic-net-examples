using System;
using System.IO;
using Aspose.Email;

namespace EmailArchiveUtility
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Determine the archive directory path.
                // If a path is provided as a command‑line argument, use it; otherwise default to "Archive".
                string archivePath;
                if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
                {
                    archivePath = args[0];
                }
                else
                {
                    archivePath = "Archive";
                }

                // Guard the directory creation with a check.
                if (!Directory.Exists(archivePath))
                {
                    // Create the directory if it does not exist.
                    DirectoryInfo createdDirectory = Directory.CreateDirectory(archivePath);
                    Console.WriteLine("Created archive directory: " + createdDirectory.FullName);
                }
                else
                {
                    Console.WriteLine("Archive directory already exists: " + Path.GetFullPath(archivePath));
                }
            }
            catch (Exception ex)
            {
                // Write any error to the error stream and exit gracefully.
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}