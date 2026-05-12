using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Message with Custom Header";
                message.Body = "This is a test email.";

                // Add custom X-Tracking header with a unique identifier
                string trackingId = Guid.NewGuid().ToString();
                message.Headers.Add("X-Tracking", trackingId);

                // Define output path
                string outputPath = "output.eml";

                // Ensure the directory exists before saving
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath} with X-Tracking: {trackingId}");
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
