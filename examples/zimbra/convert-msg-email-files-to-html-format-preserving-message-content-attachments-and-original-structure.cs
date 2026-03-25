using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";
            string outputHtmlPath = "output.html";

            // Verify input MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Build HTML content
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine("<meta charset=\"UTF-8\"/>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");

                // Subject
                sb.AppendLine($"<h1>{System.Net.WebUtility.HtmlEncode(msg.Subject)}</h1>");

                // Sender
                string sender = msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown Sender";
                sb.AppendLine($"<p><strong>From:</strong> {System.Net.WebUtility.HtmlEncode(sender)}</p>");

                // Body (HTML if available, otherwise plain text)
                if (!string.IsNullOrEmpty(msg.BodyHtml))
                {
                    sb.AppendLine(msg.BodyHtml);
                }
                else
                {
                    sb.AppendLine($"<pre>{System.Net.WebUtility.HtmlEncode(msg.Body)}</pre>");
                }

                // Attachments
                foreach (MapiAttachment att in msg.Attachments)
                {
                    // Save attachment to the same directory as the HTML output
                    string attachmentPath = Path.Combine(outputDir ?? string.Empty, att.FileName);
                    try
                    {
                        att.Save(attachmentPath);
                        sb.AppendLine($"<p>Attachment: <a href=\"{System.Net.WebUtility.HtmlEncode(att.FileName)}\">{System.Net.WebUtility.HtmlEncode(att.FileName)}</a></p>");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{att.FileName}': {ex.Message}");
                    }
                }

                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                // Write HTML file
                try
                {
                    File.WriteAllText(outputHtmlPath, sb.ToString(), Encoding.UTF8);
                    Console.WriteLine($"HTML file created at: {outputHtmlPath}");
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