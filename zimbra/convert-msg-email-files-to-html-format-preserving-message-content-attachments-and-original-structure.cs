using System;
using System.IO;
using System.Text;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace MsgToHtmlConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputMsgPath = "sample.msg";
                string outputHtmlPath = "output.html";
                string attachmentsFolder = "attachments";

                // Verify input file exists
                if (!File.Exists(inputMsgPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                    return;
                }

                // Ensure attachments folder exists
                if (!Directory.Exists(attachmentsFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(attachmentsFolder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating attachments directory: {ex.Message}");
                        return;
                    }
                }

                // Load the MSG file
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                {
                    // Get HTML body; fallback to plain text if HTML is empty
                    string bodyHtml = msg.BodyHtml;
                    if (string.IsNullOrEmpty(bodyHtml))
                    {
                        bodyHtml = $"<pre>{WebUtility.HtmlEncode(msg.Body)}</pre>";
                    }

                    // Build HTML content
                    StringBuilder htmlBuilder = new StringBuilder();
                    htmlBuilder.AppendLine("<html><body>");
                    htmlBuilder.AppendLine(bodyHtml);

                    // Process attachments
                    if (msg.Attachments != null && msg.Attachments.Count > 0)
                    {
                        htmlBuilder.AppendLine("<h3>Attachments:</h3><ul>");
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            string attachmentPath = Path.Combine(attachmentsFolder, attachment.FileName);
                            try
                            {
                                attachment.Save(attachmentPath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                                continue;
                            }

                            string relativePath = Path.Combine(attachmentsFolder, attachment.FileName);
                            htmlBuilder.AppendLine($"<li><a href=\"{relativePath}\">{attachment.FileName}</a></li>");
                        }
                        htmlBuilder.AppendLine("</ul>");
                    }

                    htmlBuilder.AppendLine("</body></html>");

                    // Write HTML to file
                    try
                    {
                        File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
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
}