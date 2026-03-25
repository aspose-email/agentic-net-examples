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
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <input_msg_path> <output_html_path>");
                return;
            }

            string inputMsgPath = args[0];
            string outputHtmlPath = args[1];

            // Verify input file exists
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
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Prepare HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<meta charset=\"UTF-8\">");
                htmlBuilder.AppendLine($"<title>{System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject")}</title>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");

                // Subject
                htmlBuilder.AppendLine($"<h1>{System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject")}</h1>");

                // From
                htmlBuilder.AppendLine($"<p><strong>From:</strong> {System.Web.HttpUtility.HtmlEncode(msg.SenderName ?? msg.SenderEmailAddress ?? "Unknown")}</p>");

                // To
                htmlBuilder.AppendLine($"<p><strong>To:</strong> {System.Web.HttpUtility.HtmlEncode(msg.DisplayTo ?? "Unknown")}</p>");

                // Date
                htmlBuilder.AppendLine($"<p><strong>Date:</strong> {msg.ClientSubmitTime}</p>");

                // Body (HTML if available, otherwise plain text)
                if (!string.IsNullOrEmpty(msg.BodyHtml))
                {
                    htmlBuilder.AppendLine("<div>");
                    htmlBuilder.AppendLine(msg.BodyHtml);
                    htmlBuilder.AppendLine("</div>");
                }
                else
                {
                    htmlBuilder.AppendLine("<pre>");
                    htmlBuilder.AppendLine(System.Web.HttpUtility.HtmlEncode(msg.Body ?? string.Empty));
                    htmlBuilder.AppendLine("</pre>");
                }

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    string attachmentsFolder = Path.Combine(outputDirectory ?? Path.GetDirectoryName(outputHtmlPath) ?? ".", "attachments");
                    try
                    {
                        Directory.CreateDirectory(attachmentsFolder);
                    }
                    catch (Exception attDirEx)
                    {
                        Console.Error.WriteLine($"Error: Unable to create attachments directory – {attachmentsFolder}. {attDirEx.Message}");
                        // Continue without saving attachments
                        attachmentsFolder = null;
                    }

                    htmlBuilder.AppendLine("<h2>Attachments</h2>");
                    htmlBuilder.AppendLine("<ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        string savedFileName = attachment.FileName;
                        string savedFilePath = null;
                        if (!string.IsNullOrEmpty(attachmentsFolder))
                        {
                            savedFilePath = Path.Combine(attachmentsFolder, savedFileName);
                            try
                            {
                                attachment.Save(savedFilePath);
                            }
                            catch (Exception attEx)
                            {
                                Console.Error.WriteLine($"Error: Unable to save attachment {savedFileName}. {attEx.Message}");
                            }
                        }

                        string relativePath = savedFilePath != null ? Path.Combine("attachments", savedFileName) : "#";
                        htmlBuilder.AppendLine($"<li><a href=\"{System.Web.HttpUtility.HtmlEncode(relativePath)}\">{System.Web.HttpUtility.HtmlEncode(savedFileName)}</a></li>");
                    }
                    htmlBuilder.AppendLine("</ul>");
                }

                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlBuilder.ToString(), Encoding.UTF8);
                    Console.WriteLine($"HTML file created at: {outputHtmlPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error: Unable to write HTML file – {outputHtmlPath}. {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}