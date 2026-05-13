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
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "cleaned.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
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

                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder email."))
                {
                    placeholder.Save(inputPath);
                    Console.WriteLine($"Placeholder EML created at {inputPath}");
                }
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Remove HTML comments if the body is HTML
                if (message.IsBodyHtml && !string.IsNullOrEmpty(message.HtmlBody))
                {
                    string cleanedHtml = Regex.Replace(message.HtmlBody, "<!--.*?-->", string.Empty, RegexOptions.Singleline);
                    message.HtmlBody = cleanedHtml;
                }

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the cleaned message as a new EML file
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Cleaned EML saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
