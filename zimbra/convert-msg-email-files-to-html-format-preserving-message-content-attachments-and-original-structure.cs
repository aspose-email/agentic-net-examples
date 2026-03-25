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
            string msgPath = "sample.msg";
            string htmlPath = "output.html";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject)}</h1>");

                // From
                string from = msg.SenderName ?? msg.SenderEmailAddress ?? string.Empty;
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(from)}</p>");

                // To (recipients)
                if (msg.Recipients != null && msg.Recipients.Count > 0)
                {
                    string[] recipientNames = new string[msg.Recipients.Count];
                    for (int i = 0; i < msg.Recipients.Count; i++)
                    {
                        recipientNames[i] = msg.Recipients[i].DisplayName ?? msg.Recipients[i].EmailAddress;
                    }
                    htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(string.Join("; ", recipientNames))}</p>");
                }

                // Body (HTML if available, otherwise plain text)
                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    bodyHtml = WebUtility.HtmlEncode(msg.Body).Replace("\n", "<br/>");
                }
                htmlBuilder.AppendLine(bodyHtml);

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentFileName = attachment.FileName;
                        try
                        {
                            attachment.Save(attachmentFileName);
                            htmlBuilder.AppendLine($"<li><a href=\"{WebUtility.HtmlEncode(attachmentFileName)}\">{WebUtility.HtmlEncode(attachment.FileName)}</a></li>");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                        }
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                try
                {
                    File.WriteAllText(htmlPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML saved to {htmlPath}");
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