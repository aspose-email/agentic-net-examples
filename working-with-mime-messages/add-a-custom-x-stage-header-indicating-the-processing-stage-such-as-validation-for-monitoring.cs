using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputFilePath = "output.eml";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a mail message and add a custom X-Stage header
            using (MailMessage message = new MailMessage())
            {
                // Basic message fields
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Message with Custom Header";
                message.Body = "This message includes a custom X-Stage header for monitoring.";

                // Add custom header indicating the processing stage
                message.Headers.Add("X-Stage", "Validation");

                // Save the message to a file
                try
                {
                    message.Save(outputFilePath);
                    Console.WriteLine($"Message saved to '{outputFilePath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save the message: {ioEx.Message}");
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
