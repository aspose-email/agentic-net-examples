using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "email.html";
            string outputPdfPath = "output.pdf";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputHtmlPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(inputHtmlPath) ?? ".");
                    File.WriteAllText(inputHtmlPath, "<html><body><p>Placeholder email content.</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string? outputDir = Path.GetDirectoryName(outputPdfPath);
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

            // Load HTML email into MailMessage
            MailMessage emailMessage;
            try
            {
                emailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML email: {ex.Message}");
                return;
            }

            // Extract URLs from HTML body
            List<string> urlList = new List<string>();
            if (!string.IsNullOrEmpty(emailMessage.HtmlBody))
            {
                try
                {
                    Regex hrefRegex = new Regex(@"href\s*=\s*[""'](?<url>[^""'>]+)[""']", RegexOptions.IgnoreCase);
                    MatchCollection matches = hrefRegex.Matches(emailMessage.HtmlBody);
                    foreach (Match match in matches)
                    {
                        string url = match.Groups["url"].Value;
                        if (!urlList.Contains(url))
                        {
                            urlList.Add(url);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to extract URLs: {ex.Message}");
                    // Continue without URLs
                }
            }

            // Convert email to MHTML in memory
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                try
                {
                    emailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email as MHTML: {ex.Message}");
                    return;
                }

                // Load MHTML into Aspose.Words Document
                Document doc;
                try
                {
                    doc = new Document(mhtmlStream);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MHTML into Word document: {ex.Message}");
                    return;
                }

                // Append appendix page with URLs
                if (urlList.Count > 0)
                {
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToDocumentEnd();
                    builder.InsertBreak(BreakType.PageBreak);
                    builder.Font.Size = 16;
                    builder.Writeln("Appendix: Extracted URLs");
                    builder.Font.Size = 12;
                    foreach (string url in urlList)
                    {
                        builder.Writeln(url);
                    }
                }

                // Save as PDF
                try
                {
                    doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
                }
            }

            // Dispose MailMessage
            emailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
