using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory and file path
            string outputDirectory = "Output";
            string outputFilePath = Path.Combine(outputDirectory, "EmailWithDisclaimer.eml");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic email properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Email";

                // Original email body
                string originalBody = "Dear Customer,\n\nThank you for your inquiry.\n\nBest regards,\nSupport Team";

                // Legal disclaimer to prepend
                string disclaimer = "DISCLAIMER: This email and any attachments are confidential and intended solely for the use of the individual or entity to whom they are addressed. If you have received this email in error, please notify the sender and delete it from your system.\n\n";

                // Insert disclaimer at the beginning of the body
                message.Body = disclaimer + originalBody;

                // Save the email to a file with error handling
                try
                {
                    message.Save(outputFilePath);
                    Console.WriteLine($"Email saved to {outputFilePath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save email: {saveEx.Message}");
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
