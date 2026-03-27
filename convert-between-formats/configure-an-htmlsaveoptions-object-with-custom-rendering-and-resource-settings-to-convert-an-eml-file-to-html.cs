using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlFilePath = "sample.eml";
            string htmlFilePath = "sample.html";

            if (!File.Exists(emlFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {emlFilePath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
            {
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    // Embed resources (images, etc.) directly into the HTML
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml,
                    // Example of adding custom CSS styles
                    CssStyles = "body { font-family: Arial, sans-serif; }"
                };

                mailMessage.Save(htmlFilePath, htmlOptions);
                Console.WriteLine($"EML file converted to HTML: {htmlFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
