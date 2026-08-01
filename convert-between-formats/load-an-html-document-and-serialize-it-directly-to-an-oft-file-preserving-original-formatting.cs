using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailOftExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Author note: This example loads an HTML email and saves it as an Outlook template (OFT) preserving formatting.
                string inputPath = "input.html";
                string outputPath = "output.oft";

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

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the HTML document with default load options
                using (MailMessage mailMessage = MailMessage.Load(inputPath, new HtmlLoadOptions()))
                {
                    // Save directly to OFT format using default options
                    mailMessage.Save(outputPath, SaveOptions.DefaultOft);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
