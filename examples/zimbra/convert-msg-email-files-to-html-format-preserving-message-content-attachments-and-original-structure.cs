using System;
using System.IO;
using System.Text;
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
                        Console.Error.WriteLine($"Error creating attachments folder: {ex.Message}");
                        return;
                    }
                }

                // Load the MSG file
                using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
                {
                    // Get HTML body; fallback to plain text if HTML not available
                    string htmlBody = mapiMessage.BodyHtml;
                    if (string.IsNullOrEmpty(htmlBody))
                    {
                        string plainBody = mapiMessage.Body ?? string.Empty;
                        htmlBody = $"<pre>{System.Net.WebUtility.HtmlEncode(plainBody)}</pre>";
                    }

                    // Build HTML content
                    StringBuilder htmlBuilder = new StringBuilder();
                    htmlBuilder.AppendLine("<html><head><meta charset=\"utf-8\"/></head><body>");
                    htmlBuilder.AppendLine(htmlBody);

                    // Process attachments
                    if (mapiMessage.Attachments != null && mapiMessage.Attachments.Count > 0)
                    {
                        htmlBuilder.AppendLine("<h2>Attachments</h2><ul>");
                        foreach (MapiAttachment attachment in mapiMessage.Attachments)
                        {
                            string attachmentFilePath = Path.Combine(attachmentsFolder, attachment.FileName);
                            try
                            {
                                attachment.Save(attachmentFilePath);
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

                    // Write HTML to output file
                    try
                    {
                        File.WriteAllText(outputHtmlPath, htmlBuilder.ToString());
                        Console.WriteLine($"HTML file saved to {outputHtmlPath}");
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