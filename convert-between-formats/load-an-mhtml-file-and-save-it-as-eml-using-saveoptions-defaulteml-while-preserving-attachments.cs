using System;
using System.IO;
using Aspose.Email;

namespace MhtmlToEmlConverter
{
    // Author: Aspose.Email sample author
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output file paths
                string inputFilePath = "sample.mhtml";
                string outputFilePath = "sample.eml";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load MHTML with options to preserve attachments and embedded messages
                MhtmlLoadOptions loadOptions = new MhtmlLoadOptions
                {
                    PreserveEmbeddedMessageFormat = true,
                    PreserveTnefAttachments = true
                };

                using (MailMessage message = MailMessage.Load(inputFilePath, loadOptions))
                {
                    // Save as EML using default EML save options (preserves attachments)
                    message.Save(outputFilePath, SaveOptions.DefaultEml);
                }

                Console.WriteLine($"Conversion completed successfully. EML saved to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
