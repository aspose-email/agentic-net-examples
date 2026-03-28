using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mhtml";

            // Ensure input HTML exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create a MailMessage and set its HTML body
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.Subject = "Converted HTML to MHTML";
                mail.HtmlBody = htmlContent;

                // Configure MHTML save options to preserve original date
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    PreserveOriginalDate = true
                };

                // Save as MHTML
                try
                {
                    mail.Save(outputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
