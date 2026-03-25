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

            // Directory to save extracted attachments
            string attachmentsDirectory = "attachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure attachments directory exists
            if (!Directory.Exists(attachmentsDirectory))
            {
                Directory.CreateDirectory(attachmentsDirectory);
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Save each attachment to the attachments directory
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                    attachment.Save(attachmentPath);
                }

                // Retrieve the HTML body; if missing, fallback to plain text wrapped in <pre>
                string htmlBody = msg.BodyHtml;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    string plainBody = msg.Body ?? string.Empty;
                    string encodedBody = System.Net.WebUtility.HtmlEncode(plainBody);
                    htmlBody = $"<pre>{encodedBody}</pre>";
                }

                // Write the HTML content to the output file
                File.WriteAllText(outputHtmlPath, htmlBody);
            }

            Console.WriteLine("MSG to HTML conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}