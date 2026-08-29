using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        // Paths for input HTML and output MHTML
        string inputPath = Path.Combine(Environment.CurrentDirectory, "email.html");
        string outputPath = Path.Combine(Environment.CurrentDirectory, "email.mhtml");

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Create a placeholder HTML file if the input does not exist
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

            try
            {
                // Create a minimal EML file first (required by Aspose.Email to recognize format)
                using (MailMessage placeholderMessage = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body."))
                {
                    placeholderMessage.Save(inputPath, SaveOptions.DefaultEml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating placeholder EML: {ex.Message}");
                return;
            }

            try
            {
                // Overwrite with simple HTML content
                File.WriteAllText(inputPath, "<html><body><p>Placeholder email content.</p></body></html>");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write placeholder HTML: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the HTML email
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage mailMessage = MailMessage.Load(inputPath, loadOptions))
            {
                // Save as self‑contained MHTML
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
            }

            Console.WriteLine($"Conversion successful. MHTML saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}
