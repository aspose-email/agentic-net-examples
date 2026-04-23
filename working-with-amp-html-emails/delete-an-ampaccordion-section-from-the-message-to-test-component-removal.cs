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
            string inputPath = "input.eml";
            string outputPath = "output.eml";

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
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To.Add("recipient@example.com");
                        placeholder.Subject = "Test Email with AMP";
                        placeholder.HtmlBody = @"<html><body>
<amp-accordion>
<section>
<h4>Header</h4>
<p>Content inside accordion.</p>
</section>
</amp-accordion>
<p>Other content outside accordion.</p>
</body></html>";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, remove the amp-accordion component, and save the result.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    string html = message.HtmlBody ?? string.Empty;

                    // Remove <amp-accordion>...</amp-accordion> blocks (case‑insensitive, single line).
                    Regex accordionRegex = new Regex(@"<amp-accordion.*?</amp-accordion>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    string cleanedHtml = accordionRegex.Replace(html, string.Empty);

                    message.HtmlBody = cleanedHtml;

                    // Save the modified message.
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the email message: {ex.Message}");
                return;
            }

            Console.WriteLine($"AMP accordion sections removed. Output saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
