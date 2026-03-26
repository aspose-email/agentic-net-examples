using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "input.msg";

            // Verify that the input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Output HTML file path
            string outputHtmlPath = "output.html";

            // Directory to store extracted attachments
            string attachmentsDirectory = "attachments";

            // Ensure the attachments directory exists
            try
            {
                Directory.CreateDirectory(attachmentsDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating attachments directory: {ex.Message}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Retrieve HTML body; fallback to plain text if HTML is not available
                string htmlContent = msg.BodyHtml;
                if (string.IsNullOrEmpty(htmlContent))
                {
                    // Simple conversion of plain text to HTML
                    string plainBody = msg.Body ?? string.Empty;
                    htmlContent = $"<pre>{System.Net.WebUtility.HtmlEncode(plainBody)}</pre>";
                }

                // Write the HTML content to the output file
                try
                {
                    File.WriteAllText(outputHtmlPath, htmlContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing HTML file: {ex.Message}");
                    return;
                }

                // Extract and save each attachment
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsDirectory, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                        // Continue processing remaining attachments
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}