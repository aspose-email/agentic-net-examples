using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simulated Delivery Status Notification (DSN) email
            using (MailMessage dsnMessage = new MailMessage())
            {
                dsnMessage.From = "mailer@example.com";
                dsnMessage.To.Add("recipient@example.com");
                dsnMessage.Subject = "Delivery Status Notification (Success)";
                dsnMessage.Body = "Your message has been delivered successfully.";

                // Add DSN‑specific headers
                dsnMessage.Headers.Add("X-Message-Id", "<12345@example.com>");
                dsnMessage.Headers.Add("X-Delivery-Status", "Delivered");
                dsnMessage.Headers.Add("X-Recipient-Status", "Success");

                // Define output location
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "dsn.eml");

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Save the DSN message to an .eml file
                try
                {
                    dsnMessage.Save(outputPath);
                    Console.WriteLine($"DSN message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save DSN message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
