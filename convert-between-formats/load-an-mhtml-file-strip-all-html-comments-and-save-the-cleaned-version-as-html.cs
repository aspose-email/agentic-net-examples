using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

namespace AsposeEmailMhtmlToHtml
{
    // Author: Aspose.Email example - loads MHTML, removes HTML comments, saves as HTML.
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output file paths.
            string inputPath = "input.mhtml";
            string outputPath = "output.html";

            // Guard against missing input file.
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

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            try
            {
                // Load the MHTML message.
                using (MailMessage mailMessage = MailMessage.Load(inputPath, new MhtmlLoadOptions()))
                {
                    // Remove HTML comments from the body.
                    string originalHtml = mailMessage.HtmlBody ?? string.Empty;
                    string cleanedHtml = Regex.Replace(
                        originalHtml,
                        "<!--.*?-->",
                        string.Empty,
                        RegexOptions.Singleline);

                    mailMessage.HtmlBody = cleanedHtml;

                    // Save the cleaned message as HTML.
                    HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                    mailMessage.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"Successfully saved cleaned HTML to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
