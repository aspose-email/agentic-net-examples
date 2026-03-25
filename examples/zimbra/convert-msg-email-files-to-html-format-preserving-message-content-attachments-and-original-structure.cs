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

            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure the attachments folder exists
            if (!Directory.Exists(attachmentsFolder))
            {
                Directory.CreateDirectory(attachmentsFolder);
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject)}</h1>");

                // From
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(msg.SenderName)}</p>");

                // To (recipients)
                if (msg.Recipients != null && msg.Recipients.Count > 0)
                {
                    htmlBuilder.Append("<p><strong>To:</strong> ");
                    foreach (MapiRecipient recipient in msg.Recipients)
                    {
                        htmlBuilder.Append(WebUtility.HtmlEncode(recipient.DisplayName));
                        htmlBuilder.Append("; ");
                    }
                    htmlBuilder.AppendLine("</p>");
                }

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
                        string attachmentPath = Path.Combine(attachmentsFolder, attachment.FileName);
                        attachment.Save(attachmentPath);
                        htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(attachmentPath)}\">{WebUtility.HtmlEncode(attachment.FileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                Console.WriteLine($"HTML file saved to {outputHtmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}