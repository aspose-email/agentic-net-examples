using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
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

            // Load MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                string htmlBody = message.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    // No HTML body, save unchanged
                    message.Save(outputPath);
                    return;
                }

                // Replace broken local file links with placeholder text
                string updatedBody = Regex.Replace(htmlBody, "(href|src)=[\"']([^\"']+)[\"']", match =>
                {
                    string attribute = match.Groups[1].Value;
                    string url = match.Groups[2].Value;

                    // Skip remote URLs
                    if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                        url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        return match.Value;
                    }

                    // If the referenced file does not exist, replace with placeholder
                    if (!File.Exists(url))
                    {
                        return $"{attribute}=\"[Missing Resource]\"";
                    }

                    return match.Value;
                });

                // Update the message body with the modified HTML
                message.SetBodyContent(updatedBody, BodyContentType.Html);

                // Save the modified message
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
