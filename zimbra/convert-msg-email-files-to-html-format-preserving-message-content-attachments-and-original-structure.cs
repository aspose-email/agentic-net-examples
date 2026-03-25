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
            string outputHtmlPath = "sample.html";
            // Directory to store extracted attachments
            string attachmentsDirectory = "sample_attachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure attachments directory exists
            if (!Directory.Exists(attachmentsDirectory))
            {
                try
                {
                    Directory.CreateDirectory(attachmentsDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating attachments directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Save each attachment to the attachments directory
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {attEx.Message}");
                    }
                }

                // Build HTML representation
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");
                htmlBuilder.AppendLine($"<h1>{WebUtility.HtmlEncode(msg.Subject)}</h1>");
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {WebUtility.HtmlEncode(msg.SenderName)}</p>");
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {WebUtility.HtmlEncode(msg.DisplayTo)}</p>");

                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    // Fallback to plain text body with line breaks
                    string bodyText = WebUtility.HtmlEncode(msg.Body);
                    bodyHtml = bodyText.Replace("\n", "<br/>");
                }
                htmlBuilder.AppendLine("<div>");
                htmlBuilder.AppendLine(bodyHtml);
                htmlBuilder.AppendLine("</div>");

                // List attachments in HTML
                htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    htmlBuilder.AppendLine($"<li>{WebUtility.HtmlEncode(attachment.FileName)}</li>");
                }
                htmlBuilder.AppendLine("</ul>");
                htmlBuilder.AppendLine("</body></html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML file saved to {outputHtmlPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error writing HTML file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}