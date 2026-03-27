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
            string outputPath = "output.msg";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MailMessage instance
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Message with Alternate Views";
                message.Body = "This is the plain text body.";

                // Create an HTML alternate view for fallback rendering
                string htmlBody = "<html><body><h1>Hello</h1><p>This is an <b>HTML</b> body.</p></body></html>";
                ContentType htmlContentType = new ContentType("text/html");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, htmlContentType);
                message.AlternateViews.Add(htmlView);

                // Save the message as an MSG file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
