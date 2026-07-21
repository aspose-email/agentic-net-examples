using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates loading an HTML file and saving it as MHTML with embedded resources.
            string inputHtmlPath = "input.html";
            string outputMhtmlPath = "output.mhtml";

            // Verify input file exists
            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputHtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputHtmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMhtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load HTML with options to locate resources
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions
            {
                PathToResources = Path.GetDirectoryName(inputHtmlPath)
            };

            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, htmlLoadOptions))
            {
                // Save as MHTML using default options (preserves resources)
                mailMessage.Save(outputMhtmlPath, SaveOptions.DefaultMhtml);
            }

            Console.WriteLine($"Successfully saved MHTML to: {outputMhtmlPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
