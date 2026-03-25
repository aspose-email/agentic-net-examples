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
            string inputMsgPath = "sample.msg";
            // Output HTML file path
            string outputHtmlPath = "output.html";
            // Directory to store extracted attachments
            string attachmentsDirectory = "attachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure attachments directory exists
            if (!Directory.Exists(attachmentsDirectory))
            {
                Directory.CreateDirectory(attachmentsDirectory);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Extract and save attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                    // Save attachment to file
                    attachment.Save(attachmentPath);
                }

                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject)}</h1>");

                // From
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(msg.SenderName)}</p>");

                // To (display list)
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(msg.DisplayTo)}</p>");

                // Body (HTML if available, otherwise plain text)
                if (!string.IsNullOrEmpty(msg.BodyHtml))
                {
                    htmlBuilder.AppendLine(msg.BodyHtml);
                }
                else
                {
                    string plainBody = WebUtility.HtmlEncode(msg.Body);
                    htmlBuilder.AppendLine($"<pre>{plainBody}</pre>");
                }

                // List attachments with links
                if (msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string relativePath = Path.Combine(attachmentsDirectory, attachment.FileName).Replace("\\", "/");
                        htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(relativePath)}\">{WebUtility.HtmlEncode(attachment.FileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                // Write HTML to file
                using (StreamWriter writer = new StreamWriter(outputHtmlPath, false, Encoding.UTF8))
                {
                    writer.Write(htmlBuilder.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}