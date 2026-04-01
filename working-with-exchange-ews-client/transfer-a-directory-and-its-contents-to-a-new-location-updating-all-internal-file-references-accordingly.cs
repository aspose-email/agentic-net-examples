using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string sourceDir = @"C:\SourceDirectory";
            string destinationDir = @"C:\DestinationDirectory";

            // Verify source directory exists
            if (!Directory.Exists(sourceDir))
            {
                Console.Error.WriteLine($"Source directory does not exist: {sourceDir}");
                return;
            }

            // Ensure destination directory exists or create it
            try
            {
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                return;
            }

            // Copy all files and subdirectories
            try
            {
                foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
                {
                    string targetSubDir = dirPath.Replace(sourceDir, destinationDir);
                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }
                }

                foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
                {
                    string destFilePath = filePath.Replace(sourceDir, destinationDir);
                    try
                    {
                        File.Copy(filePath, destFilePath, true);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to copy file '{filePath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during copy operation: {ex.Message}");
                return;
            }

            // Update internal file references in copied files
            try
            {
                foreach (string filePath in Directory.GetFiles(destinationDir, "*.*", SearchOption.AllDirectories))
                {
                    // Process only text-based files to avoid corrupting binaries
                    string extension = Path.GetExtension(filePath).ToLowerInvariant();
                    if (extension == ".txt" || extension == ".html" || extension == ".htm" || extension == ".eml" || extension == ".xml")
                    {
                        try
                        {
                            string content;
                            using (StreamReader reader = new StreamReader(filePath))
                            {
                                content = reader.ReadToEnd();
                            }

                            if (content.Contains(sourceDir))
                            {
                                string updatedContent = content.Replace(sourceDir, destinationDir);
                                using (StreamWriter writer = new StreamWriter(filePath, false))
                                {
                                    writer.Write(updatedContent);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update references in file '{filePath}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during reference update: {ex.Message}");
                return;
            }

            Console.WriteLine("Directory transfer completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
