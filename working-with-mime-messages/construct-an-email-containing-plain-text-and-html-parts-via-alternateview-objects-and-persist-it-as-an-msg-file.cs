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
            // Define output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "EmailWithAlternateViews.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample email with plain text and HTML";

                // Plain text view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version of the email.",
                    Encoding.UTF8,
                    "text/plain"))
                {
                    // HTML view
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "<html><body><h1>Hello</h1><p>This is the <b>HTML</b> version of the email.</p></body></html>",
                        Encoding.UTF8,
                        "text/html"))
                    {
                        // Add views to the message
                        message.AlternateViews.Add(plainView);
                        message.AlternateViews.Add(htmlView);
                    }
                }

                // Save the message as MSG file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine("Message saved to: " + outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
