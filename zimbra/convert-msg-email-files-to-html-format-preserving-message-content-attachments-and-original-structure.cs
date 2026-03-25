using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path (adjust as needed)
            string inputMsgPath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Prepare output HTML path
            string outputHtmlPath = Path.ChangeExtension(inputMsgPath, ".html");

            // Prepare attachments directory
            string attachmentDirectory = Path.Combine(
                Path.GetDirectoryName(inputMsgPath) ?? string.Empty,
                Path.GetFileNameWithoutExtension(inputMsgPath) + "_attachments");

            try
            {
                if (!Directory.Exists(attachmentDirectory))
                {
                    Directory.CreateDirectory(attachmentDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating attachment directory: {ex.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                string htmlContent = "<html><head><meta charset=\"utf-8\"/></head><body>";

                // Subject
                htmlContent += $"<h2>{WebUtility.HtmlEncode(msg.Subject ?? string.Empty)}</h2>";

                // From
                string fromInfo = msg.SenderName ?? msg.SenderEmailAddress ?? string.Empty;
                htmlContent += $"<p><strong>From:</strong> {WebUtility.HtmlEncode(fromInfo)}</p>";

                // To
                htmlContent += $"<p><strong>To:</strong> {WebUtility.HtmlEncode(msg.DisplayTo ?? string.Empty)}</p>";

                // Body (HTML if available, otherwise plain text)
                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    bodyHtml = $"<pre>{WebUtility.HtmlEncode(msg.Body ?? string.Empty)}</pre>";
                }
                htmlContent += bodyHtml;

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlContent += "<h3>Attachments</h3><ul>";
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentPath = Path.Combine(attachmentDirectory, attachment.FileName ?? "attachment");
                        try
                        {
                            using (FileStream fs = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.Save(fs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                            continue;
                        }

                        string relativePath = Path.GetFileName(attachmentPath);
                        htmlContent += $"<li><a href=\"{WebUtility.HtmlEncode(relativePath)}\">{WebUtility.HtmlEncode(attachment.FileName ?? "attachment")}</a></li>";
                    }
                    htmlContent += "</ul>";
                }

                htmlContent += "</body></html>";

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlContent);
                    Console.WriteLine($"HTML file created: {outputHtmlPath}");
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