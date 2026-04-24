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
            string inputMhtmlPath = "input.mht";
            string outputHtmlPath = "output.html";
            string externalCssPath = "styles.css";

            // Verify input file exists
            if (!File.Exists(inputMhtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMhtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMhtmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MHTML message
            using (MailMessage message = MailMessage.Load(inputMhtmlPath, new MhtmlLoadOptions()))
            {
                string htmlBody = message.HtmlBody ?? string.Empty;

                // Extract inline <style> blocks
                string cssContent = string.Empty;
                string stylePattern = @"<style[^>]*>(.*?)</style>";
                MatchCollection styleMatches = Regex.Matches(htmlBody, stylePattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                foreach (Match match in styleMatches)
                {
                    cssContent += match.Groups[1].Value.Trim() + Environment.NewLine;
                }

                // Write extracted CSS to external file
                try
                {
                    File.WriteAllText(externalCssPath, cssContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSS file: {ex.Message}");
                    return;
                }

                // Remove all <style> blocks from HTML
                string htmlWithoutStyle = Regex.Replace(htmlBody, stylePattern, string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Insert external stylesheet reference into <head>
                string linkTag = $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.GetFileName(externalCssPath)}\" />";
                string headPattern = @"<head[^>]*>";
                if (Regex.IsMatch(htmlWithoutStyle, headPattern, RegexOptions.IgnoreCase))
                {
                    htmlWithoutStyle = Regex.Replace(htmlWithoutStyle, headPattern, m => m.Value + Environment.NewLine + linkTag, RegexOptions.IgnoreCase);
                }
                else
                {
                    // If no <head>, prepend it
                    htmlWithoutStyle = $"<head>{Environment.NewLine}{linkTag}{Environment.NewLine}</head>{Environment.NewLine}{htmlWithoutStyle}";
                }

                // Update the message body
                message.HtmlBody = htmlWithoutStyle;

                // Save as HTML
                try
                {
                    message.Save(outputHtmlPath, new HtmlSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save HTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
