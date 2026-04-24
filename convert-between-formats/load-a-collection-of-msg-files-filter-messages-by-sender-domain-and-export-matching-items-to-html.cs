using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "InputMsgs";
            string outputDirectory = "OutputHtml";

            // Ensure input directory exists; if not, create it and exit gracefully
            if (!Directory.Exists(inputDirectory))
            {
                Directory.CreateDirectory(inputDirectory);
                Console.Error.WriteLine($"Input directory '{inputDirectory}' was missing and has been created. Place MSG files there and rerun the program.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all MSG files in the input directory
            string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            if (msgFiles.Length == 0)
            {
                Console.Error.WriteLine($"No MSG files found in '{inputDirectory}'.");
                return;
            }

            // Define the sender domain to filter by
            string senderDomain = "@example.com";

            foreach (string msgFilePath in msgFiles)
            {
                try
                {
                    // Guard against missing file (should not happen after GetFiles)
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

                        Console.Error.WriteLine($"File not found: {msgFilePath}");
                        continue;
                    }

                    // Load the MSG file
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                    {
                        // Check if the sender email address ends with the desired domain
                        string senderEmail = mapiMessage.SenderEmailAddress ?? string.Empty;
                        if (!senderEmail.EndsWith(senderDomain, StringComparison.OrdinalIgnoreCase))
                        {
                            continue; // Skip non‑matching messages
                        }

                        // Convert to MailMessage for HTML export
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                        {
                            // Prepare output HTML file path
                            string outputFileName = Path.GetFileNameWithoutExtension(msgFilePath) + ".html";
                            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

                            // Save as HTML
                            HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                            mailMessage.Save(outputFilePath, htmlOptions);
                            Console.WriteLine($"Exported: {outputFilePath}");
                        }
                    }
                }
                catch (Exception exFile)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {exFile.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
