using System;
using System.IO;
using Aspose.Email;

// Author: Generated example demonstrating creation of a basic MSG email with Aspose.Email.
class Program
{
    static void Main()
    {
        try
        {
            // Define the output file path for the MSG message.
            string outputPath = "output.msg";

            // Ensure the target directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and configure the email message.
            using (MailMessage message = new MailMessage())
            {
                // Standard headers.
                message.From = new MailAddress("sender@example.com", "Sender Name");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                message.Subject = "Sample MSG Email";

                // Plain‑text body.
                message.Body = "This is a plain text body of the MSG email created with Aspose.Email.";

                // Save the message as a .msg file.
                message.Save(outputPath);
            }

            Console.WriteLine($"Message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
