using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Plain‑text body to be wrapped
            string plainTextBody = "Hello, this is a plain‑text email body.";

            // Simple HTML template with a placeholder for the body text
            string htmlTemplate = "<html><body><p>{0}</p></body></html>";

            // Build the HTML body using the template
            string htmlBody = string.Format(htmlTemplate, plainTextBody);

            // Create a MailMessage and assign both plain‑text and HTML bodies
            using (MailMessage message = new MailMessage())
            {
                message.Body = plainTextBody;
                message.HtmlBody = htmlBody;
                message.IsBodyHtml = true;

                // Define output path for the generated HTML file
                string outputPath = "email.html";

                // Ensure the target directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Write the HTML content to the file with error handling
                try
                {
                    File.WriteAllText(outputPath, htmlBody);
                    Console.WriteLine($"HTML email saved to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write HTML file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
