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
                    attachment.Save(attachmentPath);
                }

                // Get HTML body; fallback to plain text if HTML is unavailable
                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    bodyHtml = $"<pre>{WebUtility.HtmlEncode(msg.Body)}</pre>";
                }

                // Write HTML file
                using (StreamWriter writer = new StreamWriter(outputHtmlPath, false, Encoding.UTF8))
                {
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine("<html><head><meta charset=\"utf-8\" /><title>");
                    writer.WriteLine(WebUtility.HtmlEncode(msg.Subject ?? "No Subject"));
                    writer.WriteLine("</title></head><body>");

                    writer.WriteLine($"<h1>{WebUtility.HtmlEncode(msg.Subject ?? "No Subject")}</h1>");
                    writer.WriteLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown")}</p>");
                    writer.WriteLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(msg.DisplayTo)}</p>");
                    writer.WriteLine("<hr/>");
                    writer.WriteLine(bodyHtml);

                    if (msg.Attachments.Count > 0)
                    {
                        writer.WriteLine("<h2>Attachments</h2><ul>");
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            string safeFileName = WebUtility.HtmlEncode(attachment.FileName);
                            string safeLink = WebUtility.HtmlEncode(Path.Combine(attachmentsDirectory, attachment.FileName));
                            writer.WriteLine($"<li><a href=\"{safeLink}\">{safeFileName}</a></li>");
                        }
                        writer.WriteLine("</ul>");
                    }

                    writer.WriteLine("</body></html>");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}