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
            // Create a simple email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Email with Disclaimer";
                message.Body = "Hello,\nThis is the main email body.";

                // Disclaimer text to be added as a separate plain‑text alternate view
                string disclaimerText = "Disclaimer: This email is confidential.";

                // Create the plain‑text alternate view for the disclaimer
                AlternateView disclaimerView = AlternateView.CreateAlternateViewFromString(
                    disclaimerText,
                    Encoding.UTF8,
                    "text/plain");

                // Append the disclaimer view to the message
                message.AddAlternateView(disclaimerView);

                // Define output file path
                string outputPath = "output.msg";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the message to a file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputPath}");
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
