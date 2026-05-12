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
            string outputPath = "output.eml";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Add custom X-Department header
                message.Headers.Add("X-Department", "Sales");

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine("Message saved to " + outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine("Failed to save message: " + saveEx.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
