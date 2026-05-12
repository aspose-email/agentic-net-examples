using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "HTML Email with Plain‑Text Fallback";

                // Set the HTML body
                message.HtmlBody = "<html><body><h1>Hello World</h1><p>This is an <b>HTML</b> email.</p></body></html>";
                message.IsBodyHtml = true;

                // Create plain‑text alternate view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "Hello World\r\nThis is a plain‑text version of the email.",
                    Encoding.UTF8,
                    "text/plain"))
                {
                    message.AlternateViews.Add(plainView);
                }

                // Create HTML alternate view (optional, demonstrates explicit view)
                using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    message.HtmlBody,
                    Encoding.UTF8,
                    "text/html"))
                {
                    message.AlternateViews.Add(htmlView);
                }

                // Define output file path
                string outputPath = "Email_with_alternate_views.msg";

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to disk
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
