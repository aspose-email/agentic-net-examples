using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputPngPath = "output.png";

            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    File.WriteAllText(inputHtmlPath, "<html><body><p>Placeholder email content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            string outputDirectory = Path.GetDirectoryName(outputPngPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                mhtmlStream.Position = 0;

                Document document = new Document(mhtmlStream);
                ImageSaveOptions imageOptions = new ImageSaveOptions(Aspose.Words.SaveFormat.Png)
                {
                    Resolution = 300,
                    UseAntiAliasing = true,
                    UseHighQualityRendering = true
                };
                document.Save(outputPngPath, imageOptions);
            }

            Console.WriteLine($"HTML successfully converted to PNG: {outputPngPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
