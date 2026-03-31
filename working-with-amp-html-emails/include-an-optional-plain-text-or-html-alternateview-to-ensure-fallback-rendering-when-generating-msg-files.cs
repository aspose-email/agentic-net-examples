using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "sample.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the plain‑text body."))
            {
                // Set HTML body
                message.HtmlBody = "<html><body><h1>Hello</h1><p>This is the HTML body.</p></body></html>";

                // Add plain‑text alternate view
                using (AlternateView plainView = new AlternateView(message.Body, "text/plain"))
                {
                    message.AlternateViews.Add(plainView);
                }

                // Add HTML alternate view
                using (AlternateView htmlView = new AlternateView(message.HtmlBody, "text/html"))
                {
                    message.AlternateViews.Add(htmlView);
                }

                // Save as MSG with fallback alternate views
                try
                {
                    message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving message: {ex.Message}");
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
