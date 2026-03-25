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

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><body>");

                // Use BodyHtml if available, otherwise encode plain text Body
                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    bodyHtml = WebUtility.HtmlEncode(msg.Body);
                }
                htmlBuilder.AppendLine(bodyHtml);

                // Process attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments:</h3><ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentPath = Path.Combine(outputDirectory ?? string.Empty, attachment.FileName);
                        // Save attachment to the same output directory
                        attachment.Save(attachmentPath);
                        htmlBuilder.AppendLine($"<li><a href=\"{attachment.FileName}\">{attachment.FileName}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body></html>");

                // Write HTML to file
                File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                Console.WriteLine($"HTML saved to {outputHtmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}