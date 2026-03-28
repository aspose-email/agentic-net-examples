using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Define the source and target directory names
            string sourceDirectory = "OldDirectory";
            string targetDirectory = "NewDirectory";

            // Verify that the source directory exists
            if (!Directory.Exists(sourceDirectory))
            {
                Console.Error.WriteLine($"Source directory \"{sourceDirectory}\" does not exist.");
                return;
            }

            // If the target directory already exists, report and abort
            if (Directory.Exists(targetDirectory))
            {
                Console.Error.WriteLine($"Target directory \"{targetDirectory}\" already exists.");
                return;
            }

            // Rename (move) the directory
            try
            {
                Directory.Move(sourceDirectory, targetDirectory);
                Console.WriteLine($"Directory renamed from \"{sourceDirectory}\" to \"{targetDirectory}\".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to rename directory: {ex.Message}");
                return;
            }

            // Update any file system references inside files within the renamed directory
            string[] files;
            try
            {
                files = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string filePath in files)
            {
                // Ensure the file exists before processing
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"File not found: {filePath}");
                    continue;
                }

                string fileContent;
                try
                {
                    fileContent = File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read file \"{filePath}\": {ex.Message}");
                    continue;
                }

                // Replace old directory name with new directory name in the file content
                string updatedContent = fileContent.Replace(sourceDirectory, targetDirectory);

                // Write the updated content back to the file
                try
                {
                    File.WriteAllText(filePath, updatedContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write file \"{filePath}\": {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
