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
                message.Subject = "Multipart/Alternative Example";

                // Plain‑text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version of the email.", 
                    null, 
                    "text/plain");

                // HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>This is the HTML version of the email.</h1></body></html>", 
                    null, 
                    "text/html");

                // Add the alternate views to the message
                message.AddAlternateView(plainView);
                message.AddAlternateView(htmlView);

                // Define output path
                string outputPath = "MultipartAlternative_out.msg";

                // Ensure the output directory exists
                try
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the message to a file
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
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
