using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.html";

            // Ensure input EML exists; create minimal placeholder if missing
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

                try
                {
                    string placeholderContent = "From: test@example.com\r\nTo: test@example.com\r\nSubject: Test\r\n\r\nBody";
                    File.WriteAllText(inputPath, placeholderContent);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
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
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the EML message and save as HTML with custom options
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                htmlOptions.ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml;
                htmlOptions.CssStyles = "body { font-family: Arial; margin: 20px; }";
                htmlOptions.ExtractHTMLBodyResourcesAsAttachments = false;
                htmlOptions.UseRelativePathToResources = true;

                mailMessage.Save(outputPath, htmlOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
