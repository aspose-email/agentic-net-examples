using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author: Example code to filter MSG files by sender domain and export to HTML.

            string inputDirectory = "msgfiles";
            string outputDirectory = "output_html";
            string targetDomain = "@example.com";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory not found: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating MSG files: {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                try
                {
                    // Load the MSG file
                    MapiMessage mapMsg = MapiMessage.Load(msgPath);

                    // Convert to MailMessage for easier handling
                    using (MailMessage mail = mapMsg.ToMailMessage(new MailConversionOptions()))
                    {
                        string senderAddress = mail.From?.Address ?? string.Empty;

                        // Filter by sender domain
                        if (senderAddress.EndsWith(targetDomain, StringComparison.OrdinalIgnoreCase))
                        {
                            string outputFileName = Path.GetFileNameWithoutExtension(msgPath) + ".html";
                            string outputPath = Path.Combine(outputDirectory, outputFileName);

                            // Save as HTML; format inferred from extension
                            mail.Save(outputPath);
                            Console.WriteLine($"Exported: {outputPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed processing '{msgPath}': {ex.Message}");
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
