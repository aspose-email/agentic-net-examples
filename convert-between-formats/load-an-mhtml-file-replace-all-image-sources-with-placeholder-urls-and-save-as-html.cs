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
            // Input MHTML file path
            string inputPath = "input.mhtml";
            // Output HTML file path
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
            MailMessage mailMessage = MailMessage.Load(inputPath, new MhtmlLoadOptions());

            // Save as HTML (resources embedded by default)
            HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
            mailMessage.Save(outputPath, htmlOptions);

            // Read the generated HTML
            string htmlContent = File.ReadAllText(outputPath);

            // Replace all image sources (cid: or data:) with a placeholder URL
            // This regex matches src="cid:..." or src='cid:...' or src="data:..."
            string pattern = @"src\s*=\s*[""'](?:cid:|data:)[^""']*[""']";
            string replacement = @"src=""https://example.com/placeholder.png""";
            string updatedHtml = Regex.Replace(htmlContent, pattern, replacement, RegexOptions.IgnoreCase);

            // Write the updated HTML back to the file
            File.WriteAllText(outputPath, updatedHtml);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
