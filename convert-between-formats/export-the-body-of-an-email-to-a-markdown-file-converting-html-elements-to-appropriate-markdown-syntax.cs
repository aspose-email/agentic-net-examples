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
            // Input EML/MSG file path
            string inputPath = "sample.eml";

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

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Prefer HTML body; fall back to plain text if HTML is not available
                string htmlContent = message.HtmlBody ?? message.Body ?? string.Empty;

                // Convert HTML to Markdown
                string markdown = ConvertHtmlToMarkdown(htmlContent);

                // Output Markdown file path
                string outputPath = "output.md";

                // Ensure the directory for the output file exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Write Markdown content to file
                try
                {
                    File.WriteAllText(outputPath, markdown);
                    Console.WriteLine($"Markdown file created at: {outputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write markdown file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Very simple HTML‑to‑Markdown conversion (covers common tags)
    private static string ConvertHtmlToMarkdown(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        string markdown = html;

        // Replace line breaks with newlines
        markdown = Regex.Replace(markdown, @"\r\n|\r|\n", "\n");

        // Bold: <b> or <strong>
        markdown = Regex.Replace(markdown, @"<(b|strong)>(.*?)</\1>", "**$2**", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Italic: <i> or <em>
        markdown = Regex.Replace(markdown, @"<(i|em)>(.*?)</\1>", "*$2*", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Headings: <h1> to <h6>
        for (int level = 1; level <= 6; level++)
        {
            string pattern = $@"<h{level}>(.*?)</h{level}>";
            string replacement = new string('#', level) + " $1";
            markdown = Regex.Replace(markdown, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        // Links: <a href="url">text</a>
        markdown = Regex.Replace(markdown, @"<a\s+href\s*=\s*[""']([^""']+)[""']\s*>(.*?)</a>", "[$2]($1)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Images: <img src="url" alt="text" />
        markdown = Regex.Replace(markdown, @"<img\s+[^>]*src\s*=\s*[""']([^""']+)[""'][^>]*>", "![]($1)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Unordered lists: <ul><li>Item</li></ul>
        markdown = Regex.Replace(markdown, @"<ul>\s*(<li>.*?</li>\s*)+</ul>", match =>
        {
            string listHtml = match.Value;
            string listMarkdown = Regex.Replace(listHtml, @"<li>(.*?)</li>", "- $1", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return listMarkdown;
        }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Ordered lists: <ol><li>Item</li></ol>
        markdown = Regex.Replace(markdown, @"<ol>\s*(<li>.*?</li>\s*)+</ol>", match =>
        {
            string listHtml = match.Value;
            int index = 1;
            string listMarkdown = Regex.Replace(listHtml, @"<li>(.*?)</li>", m => $"{index++}. {m.Groups[1].Value}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return listMarkdown;
        }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Remove any remaining HTML tags
        markdown = Regex.Replace(markdown, @"<[^>]+>", string.Empty);

        // Decode HTML entities
        markdown = System.Net.WebUtility.HtmlDecode(markdown);

        // Trim excess whitespace
        markdown = markdown.Trim();

        return markdown;
    }
}
