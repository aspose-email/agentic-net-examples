using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path (adjust as needed)
            string inputMsgPath = "sample.msg";

            // Output HTML file path
            string outputHtmlPath = "sample.html";

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

            // Load the MSG file into a MapiMessage
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Prepare HTML content
                string htmlBody = message.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    // Fallback to plain text body if HTML is not available
                    htmlBody = $"<pre>{System.Net.WebUtility.HtmlEncode(message.Body)}</pre>";
                }

                // Create HTML file and write content
                using (StreamWriter writer = new StreamWriter(outputHtmlPath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head>");
                    writer.WriteLine("<meta charset=\"utf-8\" />");
                    writer.WriteLine($"<title>{System.Net.WebUtility.HtmlEncode(message.Subject)}</title>");
                    writer.WriteLine("</head>");
                    writer.WriteLine("<body>");
                    writer.WriteLine($"<h1>{System.Net.WebUtility.HtmlEncode(message.Subject)}</h1>");
                    writer.WriteLine($"<p><strong>From:</strong> {System.Net.WebUtility.HtmlEncode(message.SenderName)}</p>");
                    writer.WriteLine($"<p><strong>To:</strong> {System.Net.WebUtility.HtmlEncode(message.DisplayTo)}</p>");
                    writer.WriteLine("<hr/>");
                    writer.WriteLine(htmlBody);
                    writer.WriteLine("<hr/>");

                    // Process attachments
                    if (message.Attachments != null && message.Attachments.Count > 0)
                    {
                        writer.WriteLine("<h2>Attachments</h2>");
                        writer.WriteLine("<ul>");
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            try
                            {
                                string attachmentFileName = attachment.FileName;
                                if (string.IsNullOrEmpty(attachmentFileName))
                                {
                                    attachmentFileName = "attachment.bin";
                                }

                                // Save attachment to the same directory as the HTML file
                                string attachmentPath = Path.Combine(outputDirectory ?? string.Empty, attachmentFileName);
                                attachment.Save(attachmentPath);

                                // Add link to the attachment in HTML
                                writer.WriteLine($"<li><a href=\"{System.Net.WebUtility.HtmlEncode(attachmentFileName)}\">{System.Net.WebUtility.HtmlEncode(attachmentFileName)}</a></li>");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                            }
                        }
                        writer.WriteLine("</ul>");
                    }

                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }

                Console.WriteLine($"Conversion completed. HTML saved to: {outputHtmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}