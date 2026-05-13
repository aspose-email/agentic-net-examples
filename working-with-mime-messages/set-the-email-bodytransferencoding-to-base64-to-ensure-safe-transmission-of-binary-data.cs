using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output path
            string outputPath = "output.eml";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("receiver@example.com");
                message.Subject = "Sample with Base64 Body Transfer Encoding";
                message.Body = "This is the body of the email.";

                // Set the Content-Transfer-Encoding header for the body to Base64
                // HeaderCollection supports adding a HeaderType with a string value
                message.Headers.Add(HeaderType.ContentTransferEncoding, "base64");

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
