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
            string inputPath = "input.mht";
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

            // Load the MHTML file
            MhtmlLoadOptions loadOptions = new MhtmlLoadOptions();
            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                string htmlBody = message.HtmlBody ?? string.Empty;

                // Remove HTML comments (<!-- comment -->)
                string cleanedHtml = Regex.Replace(htmlBody, "<!--.*?-->", string.Empty, RegexOptions.Singleline);

                string outputPath = "output.html";
                try
                {
                    File.WriteAllText(outputPath, cleanedHtml);
                    Console.WriteLine($"Cleaned HTML saved to: {outputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write output file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
