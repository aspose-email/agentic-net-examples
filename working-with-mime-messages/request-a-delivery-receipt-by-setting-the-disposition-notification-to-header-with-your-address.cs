using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create a simple email message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "Hello, this is a test email."))
            {
                // Request a delivery receipt by setting the Disposition-Notification-To header
                message.Headers.Add(HeaderType.DispositionNotificationTo, "sender@example.com");

                // Define output file path
                string outputPath = "output.eml";

                // Ensure the directory for the output file exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the message to a file with error handling
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}' with delivery receipt request.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
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
