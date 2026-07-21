using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file path
            string sourcePath = "source.eml";
            // Desired HTML output path
            string outputPath = "output.html";

            // Verify the source file exists
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source file '{sourcePath}' not found.");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Configure HTML save options to embed resources (images, attachments) into the HTML
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                // Save the message as HTML
                mailMessage.Save(outputPath, htmlOptions);
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}
