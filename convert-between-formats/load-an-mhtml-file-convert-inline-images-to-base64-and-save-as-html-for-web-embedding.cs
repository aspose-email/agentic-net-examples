using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools; // For HtmlSaveOptions and ResourceRenderingMode

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample demonstrates loading an MHTML file,
            // embedding its inline images as base64, and saving the result as HTML.

            const string inputPath = "input.mhtml";
            const string outputPath = "output.html";

            // Ensure the input file exists before attempting to load it.
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MHTML message with default load options.
            using (MailMessage message = MailMessage.Load(inputPath, new MhtmlLoadOptions()))
            {
                // Configure HTML save options to embed resources (images) as base64.
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                // Save the message as an HTML file with embedded images.
                message.Save(outputPath, htmlOptions);
                Console.WriteLine($"HTML file saved successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
