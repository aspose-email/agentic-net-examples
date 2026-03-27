using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "sample.msg";
            string outputHtmlPath = "output.html";

            // Verify input MSG file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Prepare HTML content
                string bodyContent = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyContent))
                {
                    // Fallback to plain text body if HTML is not available
                    bodyContent = $"<pre>{System.Net.WebUtility.HtmlEncode(msg.Body)}</pre>";
                }

                // Begin building the HTML document
                string html = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset=\"UTF-8\">\n<title>"
                              + System.Net.WebUtility.HtmlEncode(msg.Subject ?? "Message")
                              + "</title>\n</head>\n<body>\n";

                html += "<h1>" + System.Net.WebUtility.HtmlEncode(msg.Subject ?? "(No Subject)") + "</h1>\n";
                html += "<h3>From: " + System.Net.WebUtility.HtmlEncode(msg.SenderName ?? msg.SenderEmailAddress) + "</h3>\n";
                html += "<h3>To: " + System.Net.WebUtility.HtmlEncode(msg.DisplayTo) + "</h3>\n";

                html += "<div>" + bodyContent + "</div>\n";

                // Process attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    html += "<h2>Attachments</h2>\n<ul>\n";
                    int attachmentIndex = 0;
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string safeFileName = Path.GetFileName(attachment.FileName);
                        if (string.IsNullOrEmpty(safeFileName))
                        {
                            safeFileName = $"attachment_{attachmentIndex}";
                        }

                        string attachmentPath = Path.Combine(Path.GetDirectoryName(outputHtmlPath) ?? "", safeFileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Warning: Failed to save attachment '{safeFileName}' – {attEx.Message}");
                            continue;
                        }

                        html += $"<li><a href=\"{System.Net.WebUtility.HtmlEncode(safeFileName)}\">{System.Net.WebUtility.HtmlEncode(safeFileName)}</a></li>\n";
                        attachmentIndex++;
                    }
                    html += "</ul>\n";
                }

                html += "\n</body>\n</html>";

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, html);
                    Console.WriteLine($"HTML document created at: {outputHtmlPath}");
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