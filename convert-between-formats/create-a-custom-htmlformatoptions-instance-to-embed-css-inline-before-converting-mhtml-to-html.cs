using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailMhtmlToHtml
{
    // Author: Generated example for embedding CSS inline when converting MHTML to HTML.
    class Program
    {
        static void Main()
        {
            try
            {
                // Input MHTML file path
                string inputPath = "input.mhtml";

                // Verify input file exists
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

                // Load the MHTML message
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Create HtmlSaveOptions with custom inline CSS
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                    {
                        CssStyles = "body { font-family: Arial, sans-serif; color: #333; } " +
                                    "img { max-width: 100%; height: auto; }"
                    };

                    // Output HTML file path
                    string outputPath = "output.html";

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the message as HTML with the custom options
                    message.Save(outputPath, htmlOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
