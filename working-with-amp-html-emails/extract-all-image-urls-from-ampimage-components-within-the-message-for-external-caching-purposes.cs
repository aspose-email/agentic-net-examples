using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "message.eml";

            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            using (MailMessage baseMessage = MailMessage.Load(emlPath))
            {
                AmpMessage ampMessage = baseMessage as AmpMessage;
                if (ampMessage == null)
                {
                    Console.Error.WriteLine("The loaded message is not an AMP message.");
                    return;
                }

                string ampHtml = ampMessage.AmpHtmlBody;
                if (string.IsNullOrEmpty(ampHtml))
                {
                    Console.Error.WriteLine("AMP HTML body is empty.");
                    return;
                }

                List<string> imageUrls = new List<string>();
                // Regex to match <amp-img ... src="..."> (case-insensitive)
                Regex imgRegex = new Regex(@"<amp-img\b[^>]*\bsrc\s*=\s*[""'](?<url>[^""'>]+)[""']", RegexOptions.IgnoreCase);
                MatchCollection matches = imgRegex.Matches(ampHtml);
                foreach (Match match in matches)
                {
                    string url = match.Groups["url"].Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        imageUrls.Add(url);
                    }
                }

                Console.WriteLine("Extracted image URLs:");
                foreach (string url in imageUrls)
                {
                    Console.WriteLine(url);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
