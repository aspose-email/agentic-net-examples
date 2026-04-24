using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.mht";
            string outputPath = "output.html";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MHTML message
            using (MailMessage message = MailMessage.Load(inputPath, new MhtmlLoadOptions()))
            {
                // Ensure the message contains HTML body
                if (string.IsNullOrEmpty(message.HtmlBody))
                {
                    Console.Error.WriteLine("The loaded message does not contain an HTML body.");
                    return;
                }

                // Replace all image sources with a placeholder URL
                string pattern = @"src\s*=\s*""[^""]+""";
                string replacement = @"src=""https://example.com/placeholder.png""";
                string updatedHtml = Regex.Replace(message.HtmlBody, pattern, replacement, RegexOptions.IgnoreCase);

                // Assign the modified HTML back to the message
                message.HtmlBody = updatedHtml;

                // Save as HTML
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.None
                };

                message.Save(outputPath, saveOptions);
                Console.WriteLine($"HTML file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
