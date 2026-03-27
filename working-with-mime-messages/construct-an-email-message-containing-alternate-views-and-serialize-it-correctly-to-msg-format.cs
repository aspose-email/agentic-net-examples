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
            // Output file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample email with alternate views";

                // Plain text alternate view
                string plainText = "This is the plain text version of the email.";
                ContentType plainContentType = new ContentType("text/plain");
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainText, plainContentType);
                message.AlternateViews.Add(plainView);

                // HTML alternate view
                string htmlText = "<html><body><h1>Hello</h1><p>This is the HTML version.</p></body></html>";
                ContentType htmlContentType = new ContentType("text/html");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlText, htmlContentType);
                message.AlternateViews.Add(htmlView);

                // Save the message as MSG (Unicode format)
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
