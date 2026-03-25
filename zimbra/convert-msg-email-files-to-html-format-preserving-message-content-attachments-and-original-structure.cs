using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgToHtml
{
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
                    Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
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
                        Console.Error.WriteLine($"Error: Unable to create attachments directory – {dirEx.Message}");
                        return;
                    }
                }

                // Load the MSG file into a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
                {
                    // Retrieve HTML body if available; otherwise convert plain text body to HTML
                    string bodyContent = mapiMessage.BodyHtml;
                    if (string.IsNullOrEmpty(bodyContent))
                    {
                        // Encode plain text body to HTML
                        bodyContent = $"<pre>{System.Net.WebUtility.HtmlEncode(mapiMessage.Body)}</pre>";
                    }

                    // Build HTML content
                    StringBuilder htmlBuilder = new StringBuilder();
                    htmlBuilder.AppendLine("<!DOCTYPE html>");
                    htmlBuilder.AppendLine("<html>");
                    htmlBuilder.AppendLine("<head>");
                    htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
                    htmlBuilder.AppendLine($"<title>{System.Net.WebUtility.HtmlEncode(mapiMessage.Subject ?? "Message")}</title>");
                    htmlBuilder.AppendLine("</head>");
                    htmlBuilder.AppendLine("<body>");
                    htmlBuilder.AppendLine($"<h2>{System.Net.WebUtility.HtmlEncode(mapiMessage.Subject ?? "No Subject")}</h2>");
                    htmlBuilder.AppendLine($"<p><strong>From:</strong> {System.Net.WebUtility.HtmlEncode(mapiMessage.SenderName ?? "Unknown")}</p>");
                    htmlBuilder.AppendLine($"<p><strong>To:</strong> {System.Net.WebUtility.HtmlEncode(mapiMessage.DisplayTo ?? "Unknown")}</p>");
                    htmlBuilder.AppendLine("<hr/>");
                    htmlBuilder.AppendLine(bodyContent);
                    htmlBuilder.AppendLine("<hr/>");
                    htmlBuilder.AppendLine("<h3>Attachments</h3>");
                    htmlBuilder.AppendLine("<ul>");

                    // Extract and list attachments
                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        string safeFileName = Path.GetFileName(attachment.FileName);
                        string attachmentPath = Path.Combine(attachmentsDirectory, safeFileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                            string relativePath = Path.Combine(attachmentsDirectory, safeFileName).Replace("\\", "/");
                            htmlBuilder.AppendLine($"<li><a href=\"{relativePath}\">{System.Net.WebUtility.HtmlEncode(safeFileName)}</a></li>");
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Warning: Failed to save attachment '{safeFileName}' – {attEx.Message}");
                        }
                    }

                    htmlBuilder.AppendLine("</ul>");
                    htmlBuilder.AppendLine("</body>");
                    htmlBuilder.AppendLine("</html>");

                    // Write HTML to output file
                    try
                    {
                        File.WriteAllText(outputHtmlPath, htmlBuilder.ToString(), Encoding.UTF8);
                        Console.WriteLine($"HTML file generated successfully: {outputHtmlPath}");
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
}