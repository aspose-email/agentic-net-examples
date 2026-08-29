using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample loads an MHTML file, replaces all font families with Arial, and saves as MSG.
            string inputPath = "input.mhtml";
            string outputPath = "output.msg";

            // Guard input file existence
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

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MHTML message
            MailMessage message = MailMessage.Load(inputPath, new MhtmlLoadOptions());

            // Replace all font-family declarations with Arial
            string htmlBody = message.HtmlBody ?? string.Empty;
            string updatedHtml = Regex.Replace(
                htmlBody,
                @"font-family\s*:\s*[^;""']+",
                "font-family: Arial",
                RegexOptions.IgnoreCase);

            message.HtmlBody = updatedHtml;

            // Save as MSG format
            message.Save(outputPath, SaveOptions.DefaultMsg);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
