using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Simple example to create and save a MailMessage as MSG.
            string outputPath = "output.msg";

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and populate the MailMessage.
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com", "Sender");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                message.Subject = "Test Message";
                message.Body = "This is a test email saved as MSG.";

                // Save the message in MSG format.
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
