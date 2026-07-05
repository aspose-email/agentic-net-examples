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
            // Define output file path
            string outputPath = "NestedAlternative.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a mail message with nested multipart/alternative (plain text + HTML)
            MailMessage message = new MailMessage
            {
                From = "sender@example.com",
                To = "receiver@example.com",
                Subject = "Nested multipart/alternative test"
            };

            // Plain text part
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is the plain text version of the email.",
                null,
                "text/plain");

            // HTML part
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "<html><body><h1>This is the HTML version of the email.</h1></body></html>",
                null,
                "text/html");

            // Add both views to the message (multipart/alternative)
            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);

            // Save the message to MSG format
            message.Save(outputPath, SaveOptions.DefaultMsgUnicode);

            // Load the saved message to verify correct part selection
            using (MailMessage loaded = MailMessage.Load(outputPath))
            {
                // Aspose.Email selects the best view (HTML) as the Body when both are present
                Console.WriteLine("Loaded message body:");
                Console.WriteLine(loaded.Body);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
