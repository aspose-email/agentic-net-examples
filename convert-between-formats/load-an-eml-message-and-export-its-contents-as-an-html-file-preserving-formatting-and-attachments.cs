using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output paths
            string inputPath = "sample.eml";
            string outputPath = "sample.html";

            // Ensure the input EML file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                try
                {
                    string placeholder = "Subject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the EML message with default load options
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath, new EmlLoadOptions()))
                {
                    // Configure HTML save options to embed resources (images, etc.)
                    HtmlSaveOptions saveOptions = new HtmlSaveOptions
                    {
                        ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                    };

                    // Save the message as HTML
                    message.Save(outputPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing EML file: {ex.Message}");
                return;
            }

            Console.WriteLine($"EML message successfully exported to HTML: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
