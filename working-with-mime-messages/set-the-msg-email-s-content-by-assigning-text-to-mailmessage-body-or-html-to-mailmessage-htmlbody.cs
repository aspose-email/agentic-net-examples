using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This example creates a MailMessage, sets its body, and saves it as a MSG file.
            string outputPath = "example.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MailMessage instance
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Sample Message";

            // Set plain‑text body
            message.Body = "This is the plain text body of the email.";

            // Set HTML body (optional)
            message.HtmlBody = "<html><body><h1>Hello</h1><p>This is an <b>HTML</b> body.</p></body></html>";

            // Save the message as a .msg file
            message.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
