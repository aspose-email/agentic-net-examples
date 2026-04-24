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
            string outputPath = "output.html";

            // Verify input file exists
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
                // Load the MHTML message
                MhtmlLoadOptions loadOptions = new MhtmlLoadOptions();
                using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
                {
                    // Get the HTML body
                    string htmlBody = message.HtmlBody ?? string.Empty;

                    // Remove all <script>...</script> tags
                    string cleanedHtml = Regex.Replace(
                        htmlBody,
                        "<script[^>]*?>.*?</script>",
                        string.Empty,
                        RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Update the message body
                    message.HtmlBody = cleanedHtml;

                    // Save as cleaned HTML
                    HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                    saveOptions.MailMessageSaveType = MailMessageSaveType.HtmlFormat;
                    message.Save(outputPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
