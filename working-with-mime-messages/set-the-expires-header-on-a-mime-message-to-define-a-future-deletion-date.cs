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
            string outputPath = "message.eml";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Message with Expires Header";
                message.Body = "This message includes an Expires header to indicate a future deletion date.";

                // Set the Expires header to a future date (e.g., 7 days from now)
                DateTime expiresDateUtc = DateTime.UtcNow.AddDays(7);
                string expiresHeaderValue = expiresDateUtc.ToString("R"); // RFC1123 pattern
                message.Headers.Add("Expires", expiresHeaderValue);

                // Save the message to an EML file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}' with Expires header set to {expiresHeaderValue}.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
