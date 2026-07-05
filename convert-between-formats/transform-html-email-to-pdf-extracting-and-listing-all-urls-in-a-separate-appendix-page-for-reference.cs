using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        const string inputPath = "input.eml";
        const string outputPdfPath = "output.pdf";

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
                        placeholder.Save(inputPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

            try
            {
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                return;
            }
        }

        try
        {
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        try
        {
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string htmlBody = message.HtmlBody ?? message.Body ?? string.Empty;

                List<string> urls = new List<string>();
                if (!string.IsNullOrEmpty(htmlBody))
                {
                    Regex urlRegex = new Regex(@"https?://[^\s\""]+", RegexOptions.IgnoreCase);
                    foreach (Match match in urlRegex.Matches(htmlBody))
                    {
                        if (!urls.Contains(match.Value))
                        {
                            urls.Add(match.Value);
                        }
                    }
                }

                string appendixHtml = "<h2>Appendix: Extracted URLs</h2>";
                if (urls.Count > 0)
                {
                    appendixHtml += "<ul>";
                    foreach (string url in urls)
                    {
                        appendixHtml += $"<li><a href=\"{url}\">{url}</a></li>";
                    }
                    appendixHtml += "</ul>";
                }
                else
                {
                    appendixHtml += "<p>No URLs found.</p>";
                }

                string combinedHtml = htmlBody + "<hr/>" + appendixHtml;

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(combinedHtml)))
                {
                    var loadOptions = new Aspose.Words.Loading.LoadOptions
                    {
                        LoadFormat = LoadFormat.Html
                    };
                    Document doc = new Document(ms, loadOptions);
                    doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                }

                Console.WriteLine($"Email successfully converted to PDF with appendix: {outputPdfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing email: {ex.Message}");
        }
    }
}
