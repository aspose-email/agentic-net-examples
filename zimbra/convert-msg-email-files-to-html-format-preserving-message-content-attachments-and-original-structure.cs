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
            // Input MSG file path (replace with actual path or obtain from args)
            string inputMsgPath = "sample.msg";
            // Output HTML file path
            string outputHtmlPath = "output.html";

            // Validate input file existence
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}. {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();

                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html><head><meta charset=\"UTF-8\"><title>" + WebUtility.HtmlEncode(message.Subject ?? "No Subject") + "</title></head><body>");

                // Header information
                htmlBuilder.AppendLine("<h2>" + WebUtility.HtmlEncode(message.Subject ?? "No Subject") + "</h2>");
                htmlBuilder.AppendLine("<p><strong>From:</strong> " + WebUtility.HtmlEncode(message.SenderName ?? message.SenderEmailAddress ?? "Unknown") + "</p>");
                htmlBuilder.AppendLine("<p><strong>To:</strong> " + WebUtility.HtmlEncode(message.DisplayTo ?? "Unknown") + "</p>");
                htmlBuilder.AppendLine("<p><strong>Date:</strong> " + (message.ClientSubmitTime != DateTime.MinValue ? message.ClientSubmitTime.ToString() : "Unknown") + "</p>");

                // Message body
                string bodyHtml = message.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    bodyHtml = WebUtility.HtmlEncode(message.Body ?? string.Empty);
                    bodyHtml = "<pre>" + bodyHtml + "</pre>";
                }
                htmlBuilder.AppendLine("<div>" + bodyHtml + "</div>");

                // Attachments
                if (message.Attachments != null && message.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments</h3><ul>");
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string attachmentPath = Path.Combine(outputDirectory ?? string.Empty, attachment.FileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error: Unable to save attachment {attachment.FileName}. {ex.Message}");
                            continue;
                        }

                        string relativePath = Path.GetFileName(attachmentPath);
                        htmlBuilder.AppendLine("<li><a href=\"" + WebUtility.UrlEncode(relativePath) + "\">" + WebUtility.HtmlEncode(attachment.FileName) + "</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to write HTML file – {outputHtmlPath}. {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}