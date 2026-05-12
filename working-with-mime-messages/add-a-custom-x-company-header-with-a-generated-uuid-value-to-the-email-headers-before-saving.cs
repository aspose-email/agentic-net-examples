using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.eml");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new email message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Email with Custom Header";
                message.Body = "This email contains a custom X-Company-Header.";

                // Add custom header with generated UUID
                string uuid = Guid.NewGuid().ToString();
                message.Headers.Add("X-Company-Header", uuid);

                // Save the message to file
                message.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
