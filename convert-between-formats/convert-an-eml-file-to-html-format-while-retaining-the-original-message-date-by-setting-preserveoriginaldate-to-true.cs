using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.html";

            // Ensure the input EML file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a minimal placeholder EML file
                    string placeholder = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\nDate: Thu, 1 Jan 1970 00:00:00 +0000\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Configure save options to preserve the original date
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    PreserveOriginalDate = true
                };

                // Save as HTML (MHT format with .html extension)
                try
                {
                    mailMessage.Save(outputPath, saveOptions);
                    Console.WriteLine($"EML file converted to HTML and saved as '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving HTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
