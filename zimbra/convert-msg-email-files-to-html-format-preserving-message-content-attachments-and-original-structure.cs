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
            string inputMsgPath = "sample.msg";
            string outputHtmlPath = "output.html";
            string attachmentsFolder = "attachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure attachments folder exists
            if (!Directory.Exists(attachmentsFolder))
            {
                try
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating attachments folder: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject)}</h1>");

                // From
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(msg.SenderName)}</p>");

                // To
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(msg.DisplayTo)}</p>");

                // Body (HTML if available, otherwise plain text)
                string bodyContent = string.IsNullOrEmpty(msg.BodyHtml) ? WebUtility.HtmlEncode(msg.Body) : msg.BodyHtml;
                htmlBuilder.AppendLine("<div>");
                htmlBuilder.AppendLine(bodyContent);
                htmlBuilder.AppendLine("</div>");

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string safeFileName = Path.GetFileName(attachment.FileName);
                        string attachmentPath = Path.Combine(attachmentsFolder, safeFileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{safeFileName}': {ex.Message}");
                            continue;
                        }
                        htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(attachmentPath)}\">{WebUtility.HtmlEncode(safeFileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML saved to {outputHtmlPath}");
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