using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailAttachmentConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories
                string inputDirectory = "InputMessages";
                string outputDirectory = "OutputMessages";

                // Ensure input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                // Ensure output directory exists or create it
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Process each .msg file in the input directory
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Failed to enumerate files: {fileEx.Message}");
                    return;
                }

                foreach (string msgFilePath in msgFiles)
                {
                    // Guard against missing file (should not happen after enumeration)
                    if (!File.Exists(msgFilePath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found, skipping: {msgFilePath}");
                        continue;
                    }

                    try
                    {
                        // Load the Outlook message
                        using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                        {
                            // Skip messages without attachments
                            if (mapiMessage.Attachments == null || mapiMessage.Attachments.Count == 0)
                            {
                                Console.WriteLine($"No attachments found, skipping: {Path.GetFileName(msgFilePath)}");
                                continue;
                            }

                            // Convert to MailMessage
                            MailConversionOptions conversionOptions = new MailConversionOptions();
                            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                            {
                                // Build output file path (same name with .eml extension)
                                string outputFileName = Path.GetFileNameWithoutExtension(msgFilePath) + ".eml";
                                string outputPath = Path.Combine(outputDirectory, outputFileName);

                                // Save as EML
                                mailMessage.Save(outputPath, SaveOptions.DefaultEml);
                                Console.WriteLine($"Converted with attachments: {outputFileName}");
                            }
                        }
                    }
                    catch (Exception msgEx)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgFilePath}': {msgEx.Message}");
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
}
