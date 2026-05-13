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
            // Prepare the output file path
            string outputPath = "ReplyToExample.eml";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set the sender (From) address
                message.From = new MailAddress("sender@example.com", "Sender Name");

                // Set the primary recipient
                message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));

                // Set the Reply-To address (different from the sender)
                message.ReplyToList.Add(new MailAddress("replyto@example.com", "Reply-To Name"));

                // Set subject and body
                message.Subject = "Sample message with custom Reply-To";
                message.Body = "This email demonstrates setting a separate Reply-To address.";

                // Save the message to an EML file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
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
