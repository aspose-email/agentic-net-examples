using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Tools;

class Program
{
    static void Main()
    {
        const string inputPath = "input.eml";
        const string outputPath = "output.txt";

        // Ensure the input file exists; create a minimal placeholder if missing.
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

            try
            {
                var placeholder = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body.");
                placeholder.Save(inputPath, SaveOptions.DefaultEml);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the email message.
            MailMessage message = MailMessage.Load(inputPath);

            string plainText;

            if (message.IsBodyHtml)
            {
                // Get the HTML body.
                string html = message.Body;

                // Replace <img> tags with alt text or a generic placeholder.
                string htmlWithoutImages = Regex.Replace(html, "<img[^>]*>", match =>
                {
                    var altMatch = Regex.Match(match.Value, "alt\\s*=\\s*\"([^\"]*)\"", RegexOptions.IgnoreCase);
                    if (altMatch.Success)
                        return altMatch.Groups[1].Value;
                    return "[image]";
                }, RegexOptions.IgnoreCase);

                // Convert the cleaned HTML to plain text.
                plainText = StripHtml(htmlWithoutImages);
            }
            else
            {
                // Message is already plain text.
                plainText = message.Body;
            }

            // Write the plain‑text result to the output file.
            File.WriteAllText(outputPath, plainText);
            Console.WriteLine($"Plain‑text email saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing email: {ex.Message}");
        }
    }

    // Simple HTML to plain‑text conversion.
    private static string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        // Remove script and style blocks.
        string withoutScripts = Regex.Replace(html, "<(script|style)[^>]*?>.*?</\\1>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

        // Remove all remaining HTML tags.
        string withoutTags = Regex.Replace(withoutScripts, "<[^>]+>", string.Empty);

        // Decode HTML entities.
        string decoded = WebUtility.HtmlDecode(withoutTags);

        // Normalize whitespace.
        string normalized = Regex.Replace(decoded, @"\s{2,}", " ").Trim();

        return normalized;
    }
}
