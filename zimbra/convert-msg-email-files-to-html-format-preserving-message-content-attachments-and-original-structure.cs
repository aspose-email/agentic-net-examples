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
            // Input MSG file path
            string inputMsgPath = "sample.msg";

            // Output HTML file path
            string outputHtmlPath = "output.html";

            // Directory to store extracted attachments
            string attachmentsDirectory = "attachments";

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputHtmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                if (!Directory.Exists(attachmentsDirectory))
                {
                    Directory.CreateDirectory(attachmentsDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating directories: {dirEx.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Build HTML content
                StringWriter htmlWriter = new StringWriter();

                htmlWriter.WriteLine("<!DOCTYPE html>");
                htmlWriter.WriteLine("<html>");
                htmlWriter.WriteLine("<head>");
                htmlWriter.WriteLine("<meta charset=\"UTF-8\">");
                htmlWriter.WriteLine("<title>" + System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject") + "</title>");
                htmlWriter.WriteLine("</head>");
                htmlWriter.WriteLine("<body>");

                // Header information
                htmlWriter.WriteLine("<h2>" + System.Web.HttpUtility.HtmlEncode(msg.Subject ?? "No Subject") + "</h2>");
                htmlWriter.WriteLine("<p><strong>From:</strong> " + System.Web.HttpUtility.HtmlEncode(msg.SenderName ?? "Unknown") + "</p>");
                htmlWriter.WriteLine("<p><strong>To:</strong> " + System.Web.HttpUtility.HtmlEncode(msg.DisplayTo ?? "None") + "</p>");
                htmlWriter.WriteLine("<p><strong>Sent:</strong> " + (msg.ClientSubmitTime != DateTime.MinValue ? msg.ClientSubmitTime.ToString() : "Unknown") + "</p>");

                // Body
                string bodyHtml = msg.BodyHtml;
                if (string.IsNullOrEmpty(bodyHtml))
                {
                    // Fallback to plain text body
                    string plainBody = msg.Body ?? string.Empty;
                    bodyHtml = "<pre>" + System.Web.HttpUtility.HtmlEncode(plainBody) + "</pre>";
                }
                htmlWriter.WriteLine("<hr/>");
                htmlWriter.WriteLine(bodyHtml);
                htmlWriter.WriteLine("<hr/>");

                // Attachments
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    htmlWriter.WriteLine("<h3>Attachments</h3>");
                    htmlWriter.WriteLine("<ul>");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        try
                        {
                            string safeFileName = Path.GetFileName(attachment.FileName);
                            string attachmentPath = Path.Combine(attachmentsDirectory, safeFileName);
                            attachment.Save(attachmentPath);
                            string relativePath = Path.Combine(attachmentsDirectory, safeFileName).Replace("\\", "/");
                            htmlWriter.WriteLine("<li><a href=\"" + System.Web.HttpUtility.HtmlEncode(relativePath) + "\">" + System.Web.HttpUtility.HtmlEncode(safeFileName) + "</a></li>");
                        }
                        catch (Exception attEx)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {attEx.Message}");
                        }
                    }
                    htmlWriter.WriteLine("</ul>");
                }

                htmlWriter.WriteLine("</body>");
                htmlWriter.WriteLine("</html>");

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlWriter.ToString());
                    Console.WriteLine($"Conversion completed. HTML saved to: {outputHtmlPath}");
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