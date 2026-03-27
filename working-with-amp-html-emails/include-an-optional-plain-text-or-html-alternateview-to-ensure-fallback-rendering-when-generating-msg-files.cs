using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Sample Message with Alternate Views";

                // Plain‑text alternate view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain‑text version of the email.",
                    new ContentType("text/plain")))
                {
                    message.AddAlternateView(plainView);
                }

                // HTML alternate view
                using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>This is the HTML version of the email.</h1></body></html>",
                    new ContentType("text/html")))
                {
                    message.AddAlternateView(htmlView);
                }

                // Save the message as an Outlook MSG file
                message.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
