using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.txt";

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

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Work with the HTML body if present; otherwise use plain body
                string htmlBody = message.HtmlBody ?? message.Body ?? string.Empty;

                // Replace embedded image references with descriptive placeholders
                foreach (LinkedResource linkedResource in message.LinkedResources)
                {
                    string contentId = linkedResource.ContentId;
                    if (!string.IsNullOrEmpty(contentId))
                    {
                        // Use the resource name if available, otherwise the content ID
                        string placeholderName = !string.IsNullOrEmpty(linkedResource.ContentType?.Name)
                            ? linkedResource.ContentType.Name
                            : contentId;

                        string placeholder = $"[Image: {placeholderName}]";
                        string cidReference = $"cid:{contentId}";
                        htmlBody = htmlBody.Replace(cidReference, placeholder);
                    }
                }

                // Update the message's HTML body with the placeholders
                message.HtmlBody = htmlBody;

                // Convert the (modified) HTML body to plain text
                string plainText = message.GetHtmlBodyText(true);

                // Write the plain‑text result to the output file
                try
                {
                    File.WriteAllText(outputPath, plainText);
                    Console.WriteLine($"Plain‑text email saved to: {outputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write output file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
