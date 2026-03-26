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
            string inputPath = "input.msg";
            string outputPath = "output.html";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not create output directory – {outputDirectory}. {ex.Message}");
                    return;
                }
            }

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                string htmlBody = msg.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    string encodedBody = WebUtility.HtmlEncode(msg.Body ?? string.Empty);
                    htmlBody = $"<pre>{encodedBody}</pre>";
                }

                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html><head><meta charset=\"utf-8\"><title>");
                htmlBuilder.AppendLine(WebUtility.HtmlEncode(msg.Subject ?? "Message"));
                htmlBuilder.AppendLine("</title></head><body>");
                htmlBuilder.AppendLine("<h1>");
                htmlBuilder.AppendLine(WebUtility.HtmlEncode(msg.Subject ?? "No Subject"));
                htmlBuilder.AppendLine("</h1>");
                htmlBuilder.AppendLine("<h2>From: ");
                htmlBuilder.AppendLine(WebUtility.HtmlEncode(msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown"));
                htmlBuilder.AppendLine("</h2>");
                htmlBuilder.AppendLine("<div>");
                htmlBuilder.AppendLine(htmlBody);
                htmlBuilder.AppendLine("</div>");

                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments</h3><ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentFileName = attachment.FileName;
                        string attachmentPath = Path.Combine(outputDirectory ?? string.Empty, attachmentFileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Warning: Could not save attachment {attachmentFileName}. {ex.Message}");
                        }

                        htmlBuilder.AppendLine("<li>");
                        htmlBuilder.AppendLine(WebUtility.HtmlEncode(attachmentFileName));
                        htmlBuilder.AppendLine("</li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                try
                {
                    File.WriteAllText(outputPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML file saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not write HTML file – {outputPath}. {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}