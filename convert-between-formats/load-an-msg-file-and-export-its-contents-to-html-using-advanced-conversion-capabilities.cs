using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "output.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

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

            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
                {
                    MailConversionOptions conversionOptions = new MailConversionOptions();
                    conversionOptions.KeepOriginalEmailAddresses = true;

                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                    {
                        string htmlContent = mapiMessage.BodyHtml;
                        if (string.IsNullOrEmpty(htmlContent))
                        {
                            htmlContent = mailMessage.HtmlBody;
                        }

                        try
                        {
                            File.WriteAllText(outputPath, htmlContent);
                            Console.WriteLine($"HTML exported to {outputPath}");
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write HTML file: {writeEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unexpected error: {outerEx.Message}");
        }
    }
}
