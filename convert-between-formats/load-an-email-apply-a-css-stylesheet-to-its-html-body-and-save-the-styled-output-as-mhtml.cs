using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample loads an email, injects a CSS stylesheet into its HTML body, and saves it as MHTML.
            string inputPath = "input.eml";
            string outputPath = "styled_output.mhtml";

            // Guard input file existence
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

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Define CSS stylesheet
                const string css = "body { font-family: Arial, sans-serif; color: #333333; }";

                // Inject CSS into HTML body
                if (!string.IsNullOrEmpty(mailMessage.HtmlBody))
                {
                    // Simple injection: prepend a <style> block
                    mailMessage.HtmlBody = $"<style>{css}</style>{mailMessage.HtmlBody}";
                }
                else
                {
                    // If no HTML body, create a minimal one with the style
                    mailMessage.HtmlBody = $"<html><head><style>{css}</style></head><body></body></html>";
                }

                // Save as MHTML using default options
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
