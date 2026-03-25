using System;
using System.IO;
using System.Text;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputPath = "sample.msg";
            // Output HTML file path
            string outputPath = "sample.html";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                // Get HTML body if available, otherwise fallback to plain text
                string htmlBody = mapiMessage.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    htmlBody = $"<pre>{WebUtility.HtmlEncode(mapiMessage.Body)}</pre>";
                }

                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html><head><meta charset=\"utf-8\"><title>");
                htmlBuilder.AppendLine(WebUtility.HtmlEncode(mapiMessage.Subject ?? "Message"));
                htmlBuilder.AppendLine("</title></head><body>");
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(mapiMessage.Subject ?? string.Empty)}</h1>");
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(mapiMessage.SenderName ?? string.Empty)}</p>");
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(mapiMessage.DisplayTo ?? string.Empty)}</p>");
                htmlBuilder.AppendLine("<hr/>");
                htmlBuilder.AppendLine(htmlBody);

                // Process attachments
                if (mapiMessage.Attachments != null && mapiMessage.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        string attachmentPath = Path.Combine(outputDirectory ?? string.Empty, attachment.FileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                            htmlBuilder.AppendLine($"<li>{WebUtility.HtmlEncode(attachment.FileName)} - <a href=\"{WebUtility.UrlEncode(attachment.FileName)}\">Download</a></li>");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment {attachment.FileName}: {ex.Message}");
                        }
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                // Write HTML file
                try
                {
                    File.WriteAllText(outputPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML file saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing HTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}