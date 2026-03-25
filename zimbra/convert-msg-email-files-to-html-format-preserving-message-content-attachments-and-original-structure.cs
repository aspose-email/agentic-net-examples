using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputMsgPath = "sample.msg";
            string outputFolder = "output";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Save attachments to the output folder
                foreach (MapiAttachment attachment in mapiMessage.Attachments)
                {
                    string attachmentPath = Path.Combine(outputFolder, attachment.FileName);
                    attachment.Save(attachmentPath);
                }

                // Create HTML representation
                string htmlFilePath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputMsgPath) + ".html");
                using (StreamWriter writer = new StreamWriter(htmlFilePath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head>");
                    writer.WriteLine("<meta charset=\"UTF-8\">");
                    writer.WriteLine("<title>" + WebUtility.HtmlEncode(mapiMessage.Subject) + "</title>");
                    writer.WriteLine("</head>");
                    writer.WriteLine("<body>");
                    writer.WriteLine("<h1>" + WebUtility.HtmlEncode(mapiMessage.Subject) + "</h1>");
                    writer.WriteLine("<p><strong>From:</strong> " + WebUtility.HtmlEncode(mapiMessage.SenderName) + "</p>");
                    writer.WriteLine("<p><strong>To:</strong> " + WebUtility.HtmlEncode(mapiMessage.DisplayTo) + "</p>");

                    // Use the HTML body if available; otherwise fall back to plain text
                    if (!string.IsNullOrEmpty(mapiMessage.BodyHtml))
                    {
                        writer.WriteLine("<div>" + mapiMessage.BodyHtml + "</div>");
                    }
                    else
                    {
                        writer.WriteLine("<pre>" + WebUtility.HtmlEncode(mapiMessage.Body) + "</pre>");
                    }

                    // List attachments
                    if (mapiMessage.Attachments.Count > 0)
                    {
                        writer.WriteLine("<h2>Attachments</h2>");
                        writer.WriteLine("<ul>");
                        foreach (MapiAttachment attachment in mapiMessage.Attachments)
                        {
                            string safeFileName = WebUtility.HtmlEncode(attachment.FileName);
                            writer.WriteLine("<li><a href=\"" + safeFileName + "\">" + safeFileName + "</a></li>");
                        }
                        writer.WriteLine("</ul>");
                    }

                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}