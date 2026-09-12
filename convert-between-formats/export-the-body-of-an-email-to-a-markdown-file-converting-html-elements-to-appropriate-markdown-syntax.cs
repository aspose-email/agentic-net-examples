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
            // Input EML file path
            string inputPath = "input.eml";
            // Output Markdown file path
            string outputPath = "output.md";

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
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Prefer HTML body; fall back to plain text
                string htmlBody = mailMessage.HtmlBody ?? mailMessage.Body ?? string.Empty;

                // Convert HTML to Markdown
                string markdown = ConvertHtmlToMarkdown(htmlBody);

                // Write Markdown to file
                try
                {
                    File.WriteAllText(outputPath, markdown);
                    Console.WriteLine($"Markdown saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write markdown file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple HTML‑to‑Markdown conversion handling common tags
    private static string ConvertHtmlToMarkdown(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        string markdown = html;

        // Replace line breaks
        markdown = Regex.Replace(markdown, @"<(br|br\s*/?)>", "\n", RegexOptions.IgnoreCase);

        // Replace paragraphs with double line breaks
        markdown = Regex.Replace(markdown, @"</p\s*>", "\n\n", RegexOptions.IgnoreCase);
        markdown = Regex.Replace(markdown, @"<p\s*>", string.Empty, RegexOptions.IgnoreCase);

        // Bold tags
        markdown = Regex.Replace(markdown, @"<(b|strong)\s*>", "**", RegexOptions.IgnoreCase);
        markdown = Regex.Replace(markdown, @"</(b|strong)\s*>", "**", RegexOptions.IgnoreCase);

        // Italic tags
        markdown = Regex.Replace(markdown, @"<(i|em)\s*>", "*", RegexOptions.IgnoreCase);
        markdown = Regex.Replace(markdown, @"</(i|em)\s*>", "*", RegexOptions.IgnoreCase);

        // Links: <a href="url">text</a> -> [text](url)
        markdown = Regex.Replace(markdown,
            @"<a\s+href\s*=\s*[""'](?<url>[^""']+)[""']\s*>(?<text>.*?)</a>",
            m => $"[{m.Groups["text"].Value}]({m.Groups["url"].Value})",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Remove any remaining HTML tags
        markdown = Regex.Replace(markdown, @"<[^>]+>", string.Empty);

        // Decode HTML entities
        markdown = System.Net.WebUtility.HtmlDecode(markdown);

        // Trim leading/trailing whitespace
        return markdown.Trim();
    }
}
