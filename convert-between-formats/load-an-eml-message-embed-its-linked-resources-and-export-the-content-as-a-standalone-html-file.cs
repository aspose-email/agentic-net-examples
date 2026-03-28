using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "EmailWithAttachEmbedded.eml";

            // Verify that the input EML file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Load the EML message and export it as a standalone HTML file with embedded resources
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                string htmlPath = emlPath + ".html";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(htmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                message.Save(htmlPath, saveOptions);
                Console.WriteLine($"HTML file saved to: {htmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
