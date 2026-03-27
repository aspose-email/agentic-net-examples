using System;
using System.IO;
using System.Text;

namespace DirectoryTransferExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define source and destination directories
                string sourceDirectory = @"C:\SourceDir";
                string destinationDirectory = @"C:\DestDir";

                // Verify source directory exists
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.Error.WriteLine("Source directory does not exist: " + sourceDirectory);
                    return;
                }

                // Ensure destination directory exists
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Get all files from source directory recursively
                string[] sourceFiles = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                foreach (string sourceFilePath in sourceFiles)
                {
                    // Compute relative path and destination file path
                    string relativePath = Path.GetRelativePath(sourceDirectory, sourceFilePath);
                    string destinationFilePath = Path.Combine(destinationDirectory, relativePath);
                    string destinationFileDir = Path.GetDirectoryName(destinationFilePath);

                    // Ensure destination subdirectory exists
                    if (!Directory.Exists(destinationFileDir))
                    {
                        Directory.CreateDirectory(destinationFileDir);
                    }

                    // Copy file using streams
                    using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
                        {
                            sourceStream.CopyTo(destinationStream);
                        }
                    }

                    // Update internal references inside the copied file (if it's a text file)
                    try
                    {
                        // Read file content as text
                        string fileContent;
                        using (StreamReader reader = new StreamReader(destinationFilePath, Encoding.UTF8))
                        {
                            fileContent = reader.ReadToEnd();
                        }

                        // Replace occurrences of the source directory path with the destination directory path
                        string updatedContent = fileContent.Replace(sourceDirectory, destinationDirectory, StringComparison.OrdinalIgnoreCase);

                        // Write updated content back to the file
                        using (StreamWriter writer = new StreamWriter(destinationFilePath, false, Encoding.UTF8))
                        {
                            writer.Write(updatedContent);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        // If the file is not a text file or another error occurs, ignore and continue
                        Console.Error.WriteLine("Failed to update references in file: " + destinationFilePath);
                        Console.Error.WriteLine(innerEx.Message);
                    }
                }

                Console.WriteLine("Directory transfer completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}