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
            // Create a new mail message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Multipart/Alternative Email with Markdown";

            // Plain text view
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is the plain text version of the email.",
                null,
                "text/plain");

            // HTML view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "<html><body><h1>Hello</h1><p>This is the HTML version.</p></body></html>",
                null,
                "text/html");

            // Markdown view (third alternate view)
            AlternateView markdownView = AlternateView.CreateAlternateViewFromString(
                "# Hello\n\nThis is the **markdown** version of the email.\n\n- Item 1\n- Item 2",
                Encoding.UTF8,
                "text/markdown");

            // Add alternate views to the message
            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);
            message.AlternateViews.Add(markdownView);

            // Define output path
            string outputPath = "EmailWithMarkdown.msg";

            // Ensure the directory exists
            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to a file
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                Console.WriteLine($"Message saved to '{outputPath}'.");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }
            finally
            {
                // Dispose the message
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
