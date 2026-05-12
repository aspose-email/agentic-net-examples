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
            string outputPath = "sanitized.eml";

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

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                if (message.IsBodyHtml && !string.IsNullOrEmpty(message.HtmlBody))
                {
                    // Remove <script>...</script> blocks (case‑insensitive)
                    string scriptPattern = @"<script\b[^>]*>(.*?)</script>";
                    string sanitizedHtml = Regex.Replace(message.HtmlBody, scriptPattern, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    // Remove javascript: URIs
                    sanitizedHtml = Regex.Replace(sanitizedHtml, @"javascript\s*:", string.Empty, RegexOptions.IgnoreCase);

                    message.HtmlBody = sanitizedHtml;
                }

                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Sanitized message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save sanitized message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
