using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.html";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                File.WriteAllText(inputPath, "Subject: Placeholder\r\n\r\nThis is a placeholder email.");
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputPath}'.");
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the EML with UTF‑8 preferred encoding
            EmlLoadOptions loadOptions = new EmlLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            using (MailMessage mail = MailMessage.Load(inputPath, loadOptions))
            {
                // Ensure the message body uses UTF‑8 when saved
                mail.BodyEncoding = Encoding.UTF8;

                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                mail.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"Message saved as HTML to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
