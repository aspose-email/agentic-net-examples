using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input MHTML file path
            const string inputMhtmlPath = "input.mhtml";
            // Output HTML file path
            const string outputHtmlPath = "output.html";
            // External CSS file path
            const string externalCssPath = "styles.css";

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
            string outputDir = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Ensure CSS directory exists
            string cssDir = Path.GetDirectoryName(externalCssPath);
            if (!string.IsNullOrEmpty(cssDir) && !Directory.Exists(cssDir))
                Directory.CreateDirectory(cssDir);

            // Load the MHTML message
            using (MailMessage mailMessage = MailMessage.Load(inputMhtmlPath, new MhtmlLoadOptions()))
            {
                string htmlBody = mailMessage.HtmlBody ?? string.Empty;
                var cssBuilder = new StringBuilder();

                // Extract <style>...</style> blocks
                var styleBlockMatches = Regex.Matches(htmlBody, "<style[^>]*>(.*?)</style>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                foreach (Match match in styleBlockMatches)
                {
                    if (match.Groups.Count > 1)
                        cssBuilder.AppendLine(match.Groups[1].Value.Trim());
                }

                // Remove the <style> blocks from HTML
                htmlBody = Regex.Replace(htmlBody, "<style[^>]*>.*?</style>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Remove inline style attributes
                htmlBody = Regex.Replace(htmlBody, @"\sstyle\s*=\s*""[^""]*""", string.Empty, RegexOptions.IgnoreCase);

                // Write extracted CSS to external file (if any)
                if (cssBuilder.Length > 0)
                {
                    File.WriteAllText(externalCssPath, cssBuilder.ToString());
                }
                else
                {
                    // Create an empty CSS file to keep the reference valid
                    File.WriteAllText(externalCssPath, string.Empty);
                }

                // Insert link to external stylesheet after <head> tag
                if (Regex.IsMatch(htmlBody, "<head[^>]*>", RegexOptions.IgnoreCase))
                {
                    htmlBody = Regex.Replace(htmlBody, "(<head[^>]*>)", $"$1<link rel=\"stylesheet\" href=\"{Path.GetFileName(externalCssPath)}\" />", RegexOptions.IgnoreCase);
                }
                else
                {
                    // If no <head>, prepend the link at the beginning
                    htmlBody = $"<link rel=\"stylesheet\" href=\"{Path.GetFileName(externalCssPath)}\" />{Environment.NewLine}{htmlBody}";
                }

                // Update the message body
                mailMessage.HtmlBody = htmlBody;

                // Save as HTML
                var htmlSaveOptions = new HtmlSaveOptions();
                mailMessage.Save(outputHtmlPath, htmlSaveOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
