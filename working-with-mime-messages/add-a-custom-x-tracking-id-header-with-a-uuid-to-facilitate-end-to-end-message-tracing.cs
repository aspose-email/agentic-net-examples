using System;
using System.IO;
using Aspose.Email;

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
                message.Subject = "Test Message";
                message.Body = "This is a test.";

                // Add a custom X-Tracking-ID header with a UUID
                message.Headers.Add("X-Tracking-ID", Guid.NewGuid().ToString());

                // Define the output file path
                string outputPath = "TrackedMessage.eml";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to an .eml file with error handling
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
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
