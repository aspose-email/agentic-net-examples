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
            // Define input and output directories
            string inputDirectory = "InputEmls";
            string outputDirectory = "OutputEmls";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Get list of .eml files
            List<string> emlFiles = new List<string>(Directory.GetFiles(inputDirectory, "*.eml"));
            if (emlFiles.Count == 0)
            {
                Console.WriteLine("No EML files found to process.");
                return;
            }

            foreach (string emlPath in emlFiles)
            {
                // Guard each file existence
                if (!File.Exists(emlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {emlPath}");
                    continue;
                }

                try
                {
                    // Load the email message
                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        // Example policy: set sensitivity to Private if it is not already Private
                        if (message.Sensitivity != MailSensitivity.Private)
                        {
                            message.Sensitivity = MailSensitivity.Private;
                        }

                        // Determine output path
                        string fileName = Path.GetFileName(emlPath);
                        string outputPath = Path.Combine(outputDirectory, fileName);

                        // Save the modified message (overwrite if exists)
                        message.Save(outputPath);
                        Console.WriteLine($"Processed and saved: {outputPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{emlPath}': {ex.Message}");
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
