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
            string inputMsgPath = "input.msg";
            // Output HTML file path
            string outputHtmlPath = "output.html";
            // Directory to save extracted attachments
            string attachmentsDirectory = "attachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure attachments directory exists
            try
            {
                if (!Directory.Exists(attachmentsDirectory))
                {
                    Directory.CreateDirectory(attachmentsDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating attachments directory: {dirEx.Message}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Prepare HTML content
                StringBuilder htmlBuilder = new StringBuilder();

                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<meta charset=\"utf-8\" />");
                htmlBuilder.AppendLine($"<title>{WebUtility.HtmlEncode(message.Subject ?? "No Subject")}</title>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");

                // Basic metadata
                htmlBuilder.AppendLine($"<h2>{WebUtility.HtmlEncode(message.Subject ?? "No Subject")}</h2>");
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(message.SenderName ?? "Unknown")}</p>");
                htmlBuilder.AppendLine($"<p><strong>Sent:</strong> {message.ClientSubmitTime}</p>");

                // Message body (HTML if available, otherwise plain text)
                string bodyHtml = message.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    string plainBody = message.Body ?? string.Empty;
                    bodyHtml = $"<pre>{WebUtility.HtmlEncode(plainBody)}</pre>";
                }
                htmlBuilder.AppendLine("<hr/>");
                htmlBuilder.AppendLine(bodyHtml);
                htmlBuilder.AppendLine("<hr/>");

                // Attachments
                if (message.Attachments != null && message.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments</h3>");
                    htmlBuilder.AppendLine("<ul>");
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string safeFileName = Path.GetFileName(attachment.FileName);
                        string attachmentPath = Path.Combine(attachmentsDirectory, safeFileName);

                        // Save attachment to disk
                        try
                        {
                            using (FileStream attStream = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.Save(attStream);
                            }
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{safeFileName}': {attEx.Message}");
                            continue;
                        }

                        // Add link to attachment in HTML
                        string relativePath = Path.Combine(attachmentsDirectory, safeFileName).Replace("\\", "/");
                        htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(relativePath)}\">{WebUtility.HtmlEncode(safeFileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Write HTML to output file
                try
                {
                    using (FileStream htmlStream = new FileStream(outputHtmlPath, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(htmlStream, Encoding.UTF8))
                        {
                            writer.Write(htmlBuilder.ToString());
                        }
                    }
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error writing HTML file: {writeEx.Message}");
                    return;
                }

                Console.WriteLine($"Conversion completed. HTML saved to '{outputHtmlPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}