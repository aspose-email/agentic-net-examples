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

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Output HTML file path
            string outputHtmlPath = "sample.html";

            // Directory to store extracted attachments
            string attachmentsDirectory = "sample_attachments";

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

            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Retrieve HTML body; fallback to plain text if empty
                string htmlBody = msg.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    htmlBody = $"<pre>{msg.Body}</pre>";
                }

                // Write HTML content and attachment references
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputHtmlPath, false))
                    {
                        writer.WriteLine("<html>");
                        writer.WriteLine("<head><meta charset=\"utf-8\" /></head>");
                        writer.WriteLine("<body>");
                        writer.WriteLine(htmlBody);

                        // Process each attachment
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);

                            // Save attachment to file
                            try
                            {
                                attachment.Save(attachmentPath);
                            }
                            catch (Exception attEx)
                            {
                                Console.Error.WriteLine($"Warning: Failed to save attachment '{attachment.FileName}' – {attEx.Message}");
                                continue;
                            }

                            // Add a link to the attachment in the HTML
                            writer.WriteLine($"<p>Attachment: <a href=\"{Path.GetFileName(attachmentPath)}\">{attachment.FileName}</a></p>");
                        }

                        writer.WriteLine("</body>");
                        writer.WriteLine("</html>");
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error: Failed to write HTML output – {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}