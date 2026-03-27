using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string sourceDir = @"C:\SourceEmails";
            string destDir = @"C:\DestEmails";

            // Verify source directory exists
            if (!Directory.Exists(sourceDir))
            {
                Console.Error.WriteLine($"Source directory does not exist: {sourceDir}");
                return;
            }

            // Ensure destination directory exists
            try
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                return;
            }

            // Process all files recursively
            foreach (string sourcePath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                try
                {
                    // Compute relative path and target path
                    string relativePath = Path.GetRelativePath(sourceDir, sourcePath);
                    string targetPath = Path.Combine(destDir, relativePath);
                    string targetDir = Path.GetDirectoryName(targetPath);
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    // If the file is an email message, update internal references
                    if (Path.GetExtension(sourcePath).Equals(".eml", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MailMessage message = MailMessage.Load(sourcePath))
                        {
                            // Example: replace occurrences of source directory path in the body with destination path
                            if (!string.IsNullOrEmpty(message.Body))
                            {
                                string updatedBody = message.Body.Replace(sourceDir, destDir, StringComparison.OrdinalIgnoreCase);
                                message.Body = updatedBody;
                            }

                            // Save the updated message to the target location
                            message.Save(targetPath);
                        }
                    }
                    else
                    {
                        // For non-email files, simply copy
                        File.Copy(sourcePath, targetPath, overwrite: true);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{sourcePath}': {ex.Message}");
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
