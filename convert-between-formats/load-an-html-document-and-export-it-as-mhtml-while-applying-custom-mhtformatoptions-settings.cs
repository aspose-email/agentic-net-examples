using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mhtml";

            // Ensure input HTML file exists; create a minimal placeholder if missing.
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

            // Ensure output directory exists.
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

            using (MailMessage message = new MailMessage())
            {
                // Set minimal required fields.
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.HtmlBody = htmlContent;

                // Configure custom MHTML save options.
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.MhtFormatOptions = MhtFormatOptions.WriteHeader | MhtFormatOptions.WriteCompleteEmailAddress;

                try
                {
                    message.Save(outputPath, saveOptions);
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