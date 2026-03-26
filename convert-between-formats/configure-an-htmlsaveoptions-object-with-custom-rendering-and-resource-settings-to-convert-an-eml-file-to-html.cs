using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.eml";

            // Verify that the input EML file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Load the EML message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Configure HTML save options with custom rendering settings
                HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                saveOptions.ResourceRenderingMode = Aspose.Email.ResourceRenderingMode.EmbedIntoHtml;
                saveOptions.CssStyles = "body { font-family: Arial, sans-serif; }";

                string outputPath = Path.ChangeExtension(inputPath, ".html");

                // Save the message as HTML using a file stream
                using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    message.Save(outputStream, saveOptions);
                }

                Console.WriteLine($"EML file successfully converted to HTML: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}