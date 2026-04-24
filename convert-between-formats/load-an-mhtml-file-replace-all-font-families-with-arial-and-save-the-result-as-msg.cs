using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.mht";
            string outputPath = "output.msg";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            MhtmlLoadOptions loadOptions = new MhtmlLoadOptions();

            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                string html = message.HtmlBody;
                if (!string.IsNullOrEmpty(html))
                {
                    string updatedHtml = Regex.Replace(
                        html,
                        @"font-family\s*:\s*[^;""']+",
                        "font-family:Arial",
                        RegexOptions.IgnoreCase);
                    message.HtmlBody = updatedHtml;
                }

                message.Save(outputPath, SaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
