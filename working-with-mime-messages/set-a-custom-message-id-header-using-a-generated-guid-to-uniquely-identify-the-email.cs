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
            string outputPath = "output.eml";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{directory}': {ex.Message}");
                    return;
                }
            }

            // Create a new mail message
            using (MailMessage mailMessage = new MailMessage())
            {
                // Set basic properties
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Test Email with Custom Message-ID";
                mailMessage.Body = "This email contains a custom Message-ID header generated from a GUID.";

                // Generate a GUID and set it as the Message-ID header
                string guid = Guid.NewGuid().ToString();
                mailMessage.MessageId = $"<{guid}@example.com>";

                // Save the message to a file
                try
                {
                    mailMessage.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}' with Message-ID: {mailMessage.MessageId}");
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
