using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <input_msg_path> <output_html_path>");
                return;
            }

            string inputMsgPath = args[0];
            string outputHtmlPath = args[1];
            string outputDirectory = Path.GetDirectoryName(outputHtmlPath);

            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not create output directory – {outputDirectory}. {ex.Message}");
                    return;
                }
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                {
                    // Build HTML content
                    string htmlContent = "<html><head><meta charset=\"UTF-8\"><title>"
                                         + System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject")
                                         + "</title></head><body>";

                    htmlContent += "<h2>" + System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject") + "</h2>";
                    htmlContent += "<p><strong>From:</strong> " + System.Web.HttpUtility.HtmlEncode(msg.SenderName ?? "Unknown") + "</p>";
                    htmlContent += "<p><strong>To:</strong> " + System.Web.HttpUtility.HtmlEncode(string.Join(", ", msg.Recipients)) + "</p>";
                    htmlContent += "<p><strong>Date:</strong> " + (msg.ClientSubmitTime != DateTime.MinValue ? msg.ClientSubmitTime.ToString() : "Unknown") + "</p>";
                    htmlContent += "<hr/>";

                    string bodyHtml = msg.BodyHtml;
                    if (!string.IsNullOrEmpty(bodyHtml))
                    {
                        htmlContent += bodyHtml;
                    }
                    else
                    {
                        // Fallback to plain text body
                        string plainBody = System.Web.HttpUtility.HtmlEncode(msg.Body ?? string.Empty);
                        htmlContent += "<pre>" + plainBody + "</pre>";
                    }

                    // Process attachments
                    if (msg.Attachments != null && msg.Attachments.Count > 0)
                    {
                        htmlContent += "<hr/><h3>Attachments</h3><ul>";
                        int attachmentIndex = 0;
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            string attachmentFileName = string.IsNullOrEmpty(attachment.FileName) ? $"attachment_{attachmentIndex}" : attachment.FileName;
                            string attachmentPath = Path.Combine(outputDirectory, attachmentFileName);
                            try
                            {
                                attachment.Save(attachmentPath);
                                htmlContent += "<li><a href=\"" + System.Web.HttpUtility.UrlPathEncode(attachmentFileName) + "\">"
                                               + System.Web.HttpUtility.HtmlEncode(attachmentFileName) + "</a></li>";
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Warning: Could not save attachment '{attachmentFileName}'. {ex.Message}");
                            }
                            attachmentIndex++;
                        }
                        htmlContent += "</ul>";
                    }

                    htmlContent += "</body></html>";

                    // Write HTML to file
                    try
                    {
                        File.WriteAllText(outputHtmlPath, htmlContent);
                        Console.WriteLine($"HTML file created at: {outputHtmlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Could not write HTML file – {outputHtmlPath}. {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}