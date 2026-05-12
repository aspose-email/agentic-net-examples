using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the source directory containing EML files
            string sourceDirectoryPath = "Emails";
            // Define the archive directory where matching messages will be moved
            string archiveDirectoryPath = "Archive";

            // Ensure the source directory exists
            if (!Directory.Exists(sourceDirectoryPath))
            {
                Console.Error.WriteLine($"Source directory does not exist: {sourceDirectoryPath}");
                return;
            }

            // Ensure the archive directory exists; create if it does not
            if (!Directory.Exists(archiveDirectoryPath))
            {
                try
                {
                    Directory.CreateDirectory(archiveDirectoryPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create archive directory: {dirEx.Message}");
                    return;
                }
            }

            // Define the date range for filtering
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            // Get all .eml files in the source directory
            string[] emlFilePaths;
            try
            {
                emlFilePaths = Directory.GetFiles(sourceDirectoryPath, "*.eml");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Failed to enumerate EML files: {getFilesEx.Message}");
                return;
            }

            foreach (string emlFilePath in emlFilePaths)
            {
                // Guard against missing file (should not happen after GetFiles, but safe)
                if (!File.Exists(emlFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {emlFilePath}");
                    continue;
                }

                // Load the email message
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(emlFilePath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load EML file '{emlFilePath}': {loadEx.Message}");
                    continue;
                }

                // Check the SentDate property
                DateTime sentDate = mailMessage.Date;
                if (sentDate >= startDate && sentDate <= endDate)
                {
                    // Build destination path
                    string fileName = Path.GetFileName(emlFilePath);
                    string destinationPath = Path.Combine(archiveDirectoryPath, fileName);

                    // Move the file
                    try
                    {
                        // If a file with the same name already exists in the archive, overwrite it
                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath);
                        }
                        File.Move(emlFilePath, destinationPath);
                        Console.WriteLine($"Archived: {fileName}");
                    }
                    catch (Exception moveEx)
                    {
                        Console.Error.WriteLine($"Failed to move file '{emlFilePath}' to archive: {moveEx.Message}");
                    }
                }

                // Dispose of the MailMessage if it implements IDisposable
                if (mailMessage is IDisposable disposableMessage)
                {
                    disposableMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
