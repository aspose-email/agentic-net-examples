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

            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
                htmlBuilder.AppendLine("<title>" + WebUtility.HtmlEncode(mapiMessage.Subject) + "</title>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");
                htmlBuilder.AppendLine("<h1>" + WebUtility.HtmlEncode(mapiMessage.Subject) + "</h1>");
                htmlBuilder.AppendLine("<p><strong>From:</strong> " + WebUtility.HtmlEncode(mapiMessage.SenderName) + "</p>");

                // Use HTML body if available; otherwise plain text
                if (!string.IsNullOrEmpty(mapiMessage.BodyHtml))
                {
                    htmlBuilder.AppendLine(mapiMessage.BodyHtml);
                }
                else
                {
                    htmlBuilder.AppendLine("<pre>" + WebUtility.HtmlEncode(mapiMessage.Body) + "</pre>");
                }

                // Extract and save attachments, adding links to the HTML
                foreach (MapiAttachment attachment in mapiMessage.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        htmlBuilder.AppendLine("<p>Attachment: <a href=\"" + WebUtility.HtmlEncode(attachmentPath) + "\">" + WebUtility.HtmlEncode(attachment.FileName) + "</a></p>");
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {attEx.Message}");
                    }
                }

                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Write the HTML file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                    Console.WriteLine($"HTML saved to {outputHtmlPath}");
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