using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string htmlPath = "sample.html";
            string mhtmlPath = "output.mht";

            // Verify input file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Input file '{htmlPath}' does not exist.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(mhtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Read HTML content
            string htmlContent;
            using (FileStream htmlStream = new FileStream(htmlPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(htmlStream))
            {
                htmlContent = reader.ReadToEnd();
            }

            // Create MailMessage with HTML body and original date
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.HtmlBody = htmlContent;
                mailMessage.Date = File.GetLastWriteTime(htmlPath);

                // Configure MHTML save options to preserve original date
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    PreserveOriginalDate = true
                };

                // Save as MHTML
                mailMessage.Save(mhtmlPath, saveOptions);
            }

            Console.WriteLine($"MHTML file saved to '{mhtmlPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}