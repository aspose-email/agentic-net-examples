using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file path
            string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
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

                try
                {
                    string placeholderContent = "From: sender@example.com\r\n" +
                                                "To: recipient@example.com\r\n" +
                                                "Subject: Placeholder Email\r\n" +
                                                "MIME-Version: 1.0\r\n" +
                                                "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                                                "<html><body>Visit <a href=\"http://example.com\">example</a> and <a href=\"https://contoso.com/page\">contoso</a>.</body></html>";
                    File.WriteAllText(emlPath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Get the HTML body as plain text with URLs included
                string bodyWithUrls = mailMessage.GetHtmlBodyText(true);

                // Extract URLs using regular expression
                List<string> urlList = new List<string>();
                if (!string.IsNullOrEmpty(bodyWithUrls))
                {
                    Regex urlRegex = new Regex(@"https?://[^\s]+", RegexOptions.IgnoreCase);
                    MatchCollection matches = urlRegex.Matches(bodyWithUrls);
                    foreach (Match match in matches)
                    {
                        urlList.Add(match.Value);
                    }
                }

                // Output CSV file path
                string csvPath = "urls.csv";

                // Ensure the directory for the CSV exists
                try
                {
                    string csvDirectory = Path.GetDirectoryName(csvPath);
                    if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
                    {
                        Directory.CreateDirectory(csvDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to ensure CSV directory exists: {ex.Message}");
                    return;
                }

                // Write URLs to CSV
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath))
                    {
                        foreach (string url in urlList)
                        {
                            writer.WriteLine(url);
                        }
                    }
                    Console.WriteLine($"Extracted {urlList.Count} URL(s) to '{csvPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
