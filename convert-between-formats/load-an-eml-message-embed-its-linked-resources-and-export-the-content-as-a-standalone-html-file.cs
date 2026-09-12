using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This sample loads an EML file, embeds its linked resources, and saves as a standalone HTML file.
            string sourcePath = "source.eml";
            string outputPath = "source.html";

            // Verify that the source EML file exists before proceeding.
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

                Console.Error.WriteLine($"Input file not found: {sourcePath}");
                return;
            }

            // Load the EML message.
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Configure HTML save options to embed linked resources directly into the HTML.
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                // Save the message as a standalone HTML file.
                mailMessage.Save(outputPath, htmlOptions);
                Console.WriteLine($"HTML file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
