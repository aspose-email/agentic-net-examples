using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            EmlLoadOptions loadOptions = new EmlLoadOptions();

            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };
                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
