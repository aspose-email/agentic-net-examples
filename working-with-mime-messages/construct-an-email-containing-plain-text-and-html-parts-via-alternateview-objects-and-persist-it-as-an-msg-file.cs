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
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "EmailWithAlternateViews.msg");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Guard file write operation
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    // Set basic properties
                    message.From = "sender@example.com";
                    message.To = "recipient@example.com";
                    message.Subject = "Sample email with plain text and HTML";

                    // Create plain‑text alternate view
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                        "This is the plain‑text version of the email.", null, "text/plain");

                    // Create HTML alternate view
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "<html><body><h1>This is the HTML version of the email.</h1></body></html>", null, "text/html");

                    // Add alternate views to the message
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);

                    // Save the message as an MSG file
                    message.Save(outputPath);
                }

                Console.WriteLine($"Message saved successfully to: {outputPath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File operation failed: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
