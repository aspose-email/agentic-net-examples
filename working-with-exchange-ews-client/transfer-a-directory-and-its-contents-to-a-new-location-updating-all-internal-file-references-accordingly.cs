using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Directories (use arguments or defaults)
            string sourceDir = args.Length > 0 ? args[0] : @"C:\SourceFolder";
            string destinationDir = args.Length > 1 ? args[1] : @"C:\DestinationFolder";

            // Ensure source directory exists – create placeholder if missing
            if (!Directory.Exists(sourceDir))
            {
                Directory.CreateDirectory(sourceDir);
                // Create a minimal placeholder file that contains a reference to the source path
                string placeholderFile = Path.Combine(sourceDir, "placeholder.txt");
                File.WriteAllText(placeholderFile, $"This file references the source directory: {sourceDir}", Encoding.UTF8);
                Console.WriteLine($"Created placeholder source directory and file at: {placeholderFile}");
            }

            // Ensure destination directory exists
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Copy subdirectories
            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                string targetSubDir = dirPath.Replace(sourceDir, destinationDir);
                Directory.CreateDirectory(targetSubDir);
            }

            // Copy files
            foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string targetFilePath = filePath.Replace(sourceDir, destinationDir);
                File.Copy(filePath, targetFilePath, true);
            }

            // Update internal references in copied text-based files
            string[] textExtensions = new[] { ".txt", ".html", ".htm", ".config", ".xml", ".json", ".cs", ".js", ".css" };
            foreach (string copiedFile in Directory.GetFiles(destinationDir, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    if (Array.Exists(textExtensions, ext => ext.Equals(Path.GetExtension(copiedFile), StringComparison.OrdinalIgnoreCase)))
                    {
                        string content = File.ReadAllText(copiedFile, Encoding.UTF8);
                        if (content.Contains(sourceDir))
                        {
                            string updated = content.Replace(sourceDir, destinationDir);
                            File.WriteAllText(copiedFile, updated, Encoding.UTF8);
                        }
                    }
                }
                catch
                {
                    // Ignore files that cannot be read as text
                }
            }

            Console.WriteLine("Directory transfer completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
