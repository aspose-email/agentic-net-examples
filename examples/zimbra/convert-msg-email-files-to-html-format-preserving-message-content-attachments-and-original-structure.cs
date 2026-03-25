using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine input MSG file path (first argument or default placeholder)
            string inputPath = args.Length > 0 ? args[0] : "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Prepare output HTML file path
            string outputHtmlPath = Path.ChangeExtension(inputPath, ".html");
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
                htmlBuilder.AppendLine($"<title>{System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject")}</title>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");

                // Add basic metadata
                htmlBuilder.AppendLine($"<h2>{System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject")}</h2>");
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {System.Web.HttpUtility.HtmlEncode(msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown")}</p>");
                htmlBuilder.AppendLine($"<p><strong>Sent:</strong> {msg.ClientSubmitTime}</p>");

                // Add message body (HTML if available, otherwise plain text)
                string bodyHtml = msg.BodyHtml;
                if (!string.IsNullOrEmpty(bodyHtml))
                {
                    htmlBuilder.AppendLine(bodyHtml);
                }
                else if (!string.IsNullOrEmpty(msg.Body))
                {
                    htmlBuilder.AppendLine("<pre>");
                    htmlBuilder.AppendLine(System.Web.HttpUtility.HtmlEncode(msg.Body));
                    htmlBuilder.AppendLine("</pre>");
                }
                else
                {
                    htmlBuilder.AppendLine("<p>(No body content)</p>");
                }

                // Process attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlBuilder.AppendLine("<h3>Attachments</h3>");
                    htmlBuilder.AppendLine("<ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string attachmentFileName = attachment.FileName;
                        if (string.IsNullOrEmpty(attachmentFileName))
                        {
                            attachmentFileName = "attachment.bin";
                        }

                        string attachmentPath = Path.Combine(outputDirectory, attachmentFileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Warning: Failed to save attachment '{attachmentFileName}' – {attEx.Message}");
                            continue;
                        }

                        string encodedFileName = System.Web.HttpUtility.HtmlEncode(attachmentFileName);
                        htmlBuilder.AppendLine($"<li><a href=\"{encodedFileName}\">{encodedFileName}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString(), Encoding.UTF8);
                    Console.WriteLine($"Conversion completed. HTML saved to: {outputHtmlPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error: Unable to write HTML file – {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}