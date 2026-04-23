using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.mhtml";
            string outputPath = "output.html";

            // Guard against missing input file
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            try
            {
                // Load the MHTML message
                MhtmlLoadOptions loadOptions = new MhtmlLoadOptions();
                using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
                {
                    // Configure HTML save options to embed images as base64
                    HtmlSaveOptions saveOptions = new HtmlSaveOptions
                    {
                        ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                    };

                    // Save as HTML with embedded images
                    message.Save(outputPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
