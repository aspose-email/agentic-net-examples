using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputEmlPath = "output.eml";

            // Ensure input HTML exists; create minimal placeholder if missing
            try
            {
                if (!File.Exists(inputHtmlPath))
                {
                    string placeholderHtml = "<html><body><p>Placeholder email content.</p></body></html>";
                    File.WriteAllText(inputHtmlPath, placeholderHtml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare input file: {ex.Message}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputEmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load HTML as MailMessage and save as EML
            try
            {
                using (MailMessage mail = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
                {
                    mail.Save(outputEmlPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
