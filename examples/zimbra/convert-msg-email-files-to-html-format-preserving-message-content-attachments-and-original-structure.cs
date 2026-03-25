using System;
using System.IO;
using System.Net;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MSG file and output HTML file paths
            string inputMsgPath = "sample.msg";
            string outputHtmlPath = "output.html";
            string attachmentsFolder = "attachments";

            // Verify that the input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure the directory for the output HTML file exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Ensure the attachments folder exists
            if (!Directory.Exists(attachmentsFolder))
            {
                Directory.CreateDirectory(attachmentsFolder);
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Build the HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
                htmlBuilder.AppendLine("<title>" + WebUtility.HtmlEncode(msg.Subject ?? "No Subject") + "</title>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");

                // Subject
                htmlBuilder.AppendLine("<h1>" + WebUtility.HtmlEncode(msg.Subject ?? "No Subject") + "</h1>");

                // From
                string fromInfo = msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown";
                htmlBuilder.AppendLine("<p><strong>From:</strong> " + WebUtility.HtmlEncode(fromInfo) + "</p>");

                // To (recipients)
                if (msg.Recipients != null && msg.Recipients.Count > 0)
                {
                    StringBuilder toBuilder = new StringBuilder();
                    foreach (MapiRecipient recipient in msg.Recipients)
                    {
                        if (toBuilder.Length > 0)
                        {
                            toBuilder.Append(", ");
                        }
                        toBuilder.Append(recipient.DisplayName ?? recipient.EmailAddress ?? "Unknown");
                    }
                    htmlBuilder.AppendLine("<p><strong>To:</strong> " + WebUtility.HtmlEncode(toBuilder.ToString()) + "</p>");
                }

                // Body (HTML if available, otherwise plain text)
                if (!string.IsNullOrEmpty(msg.BodyHtml))
                {
                    htmlBuilder.AppendLine(msg.BodyHtml);
                }
                else
                {
                    string plainBody = msg.Body ?? string.Empty;
                    htmlBuilder.AppendLine("<pre>" + WebUtility.HtmlEncode(plainBody) + "</pre>");
                }

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h2>Attachments</h2>");
                    htmlBuilder.AppendLine("<ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentFileName = Path.GetFileName(attachment.FileName);
                        string savedAttachmentPath = Path.Combine(attachmentsFolder, attachmentFileName);
                        // Save the attachment to the attachments folder
                        attachment.Save(savedAttachmentPath);
                        string relativePath = Path.Combine(attachmentsFolder, attachmentFileName).Replace("\\", "/");
                        htmlBuilder.AppendLine("<li><a href=\"" + WebUtility.UrlEncode(relativePath) + "\">" + WebUtility.HtmlEncode(attachmentFileName) + "</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Write the HTML content to the output file
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