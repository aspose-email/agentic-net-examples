using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the mail message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Sample email with alternate views";

                // Plain‑text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain‑text version of the email.", null, "text/plain");

                // HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>Hello</h1><p>This is the <b>HTML</b> version.</p></body></html>",
                    null,
                    "text/html");

                // Add alternate views to the message
                mailMessage.AlternateViews.Add(plainView);
                mailMessage.AlternateViews.Add(htmlView);

                // Save the message as MSG
                try
                {
                    mailMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
