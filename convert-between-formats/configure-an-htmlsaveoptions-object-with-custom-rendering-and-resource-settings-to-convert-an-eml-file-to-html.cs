using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            if (!File.Exists(inputPath))
{
    try
    {
        string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
        File.WriteAllText(inputPath, placeholder);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
        return;
    }
}


            string outputPath = "sample.html";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                htmlOptions.ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml;
                htmlOptions.UseRelativePathToResources = true;
                htmlOptions.CssStyles = "body {font-family: Arial;}";
                message.Save(outputPath, htmlOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
