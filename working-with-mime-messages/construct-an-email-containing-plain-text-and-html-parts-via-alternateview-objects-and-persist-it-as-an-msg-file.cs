using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "email_output.msg";

            // Ensure the output directory exists
            try
            {
                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample email with plain text and HTML";

                // Create plain‑text alternate view
                ContentType plainContentType = new ContentType("text/plain");
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version of the email.", plainContentType);

                // Create HTML alternate view
                ContentType htmlContentType = new ContentType("text/html");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>Hello</h1><p>This is the <b>HTML</b> version of the email.</p></body></html>",
                    htmlContentType);

                // Add alternate views to the message
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Save the message as MSG using Unicode format
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                try
                {
                    message.Save(outputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"Message saved successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
