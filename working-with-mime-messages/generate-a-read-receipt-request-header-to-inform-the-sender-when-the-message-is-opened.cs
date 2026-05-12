using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test email with read receipt request";
                message.Body = "Please read this email.";

                // Add a read receipt request header (X-Confirm-Reading-To)
                message.Headers.Add(HeaderType.XConfirmReadingTo, "sender@example.com");

                // Optionally, also set the ReadReceiptTo property
                message.ReadReceiptTo.Add("sender@example.com");

                // Define output file path
                string outputPath = "ReadReceiptRequest.eml";

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message to an EML file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
