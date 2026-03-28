using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "sample.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MailMessage mailMessage = MailMessage.Load(
                inputPath,
                new MsgLoadOptions
                {
                    PreserveRtfContent = true,
                    PreserveEmbeddedMessageFormat = true
                }))
            {
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                mailMessage.Save(outputPath, htmlOptions);
                Console.WriteLine($"Message exported to HTML: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
