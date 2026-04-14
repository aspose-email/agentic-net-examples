using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists
            if (!File.Exists(msgPath))
            {
                // Create a minimal placeholder MSG file
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder",
                        "This is a placeholder message with a link http://example.com.");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
                Console.WriteLine($"Placeholder MSG file created at '{msgPath}'.");
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Prefer HTML body if available, otherwise plain text
                string bodyContent = string.IsNullOrEmpty(message.BodyHtml) ? message.Body : message.BodyHtml;

                // Extract URLs using regex
                List<string> extractedUrls = new List<string>();
                if (!string.IsNullOrEmpty(bodyContent))
                {
                    Regex urlRegex = new Regex(@"https?://[^\s""'>]+", RegexOptions.IgnoreCase);
                    MatchCollection matches = urlRegex.Matches(bodyContent);
                    foreach (Match match in matches)
                    {
                        extractedUrls.Add(match.Value);
                    }
                }

                // Define whitelist of allowed URLs
                List<string> whitelist = new List<string>
                {
                    "https://trusted.com",
                    "http://example.com"
                };

                // Validate each URL against the whitelist
                foreach (string url in extractedUrls)
                {
                    if (whitelist.Contains(url))
                    {
                        Console.WriteLine($"URL allowed: {url}");
                    }
                    else
                    {
                        Console.WriteLine($"URL NOT allowed: {url}");
                    }
                }

                if (extractedUrls.Count == 0)
                {
                    Console.WriteLine("No URLs found in the message body.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
