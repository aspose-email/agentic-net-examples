using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define source and destination directories
            string sourceDirectory = @"C:\Source";
            string destinationDirectory = @"C:\Destination";

            // Verify source directory exists
            if (!Directory.Exists(sourceDirectory))
            {
                Console.Error.WriteLine($"Source directory does not exist: {sourceDirectory}");
                return;
            }

            // Ensure destination directory exists
            try
            {
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                return;
            }

            // Get all files from source directory recursively
            string[] allFiles;
            try
            {
                allFiles = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string sourceFilePath in allFiles)
            {
                try
                {
                    // Compute relative path and destination path
                    string relativePath = Path.GetRelativePath(sourceDirectory, sourceFilePath);
                    string destinationFilePath = Path.Combine(destinationDirectory, relativePath);
                    string destinationFileDir = Path.GetDirectoryName(destinationFilePath);

                    // Ensure destination subdirectory exists
                    if (!Directory.Exists(destinationFileDir))
                    {
                        Directory.CreateDirectory(destinationFileDir);
                    }

                    // If the file is an email message (EML), load and save using Aspose.Email to update internal references
                    if (string.Equals(Path.GetExtension(sourceFilePath), ".eml", StringComparison.OrdinalIgnoreCase))
                    {
                        // Load the email message
                        using (MailMessage mailMessage = MailMessage.Load(sourceFilePath))
                        {
                            // Example placeholder: update any custom headers or properties if needed
                            // mailMessage.Headers["X-Custom-Header"] = "UpdatedValue";

                            // Save the email to the new location
                            mailMessage.Save(destinationFilePath);
                        }
                    }
                    else
                    {
                        // For non-email files, perform a simple copy
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{sourceFilePath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
