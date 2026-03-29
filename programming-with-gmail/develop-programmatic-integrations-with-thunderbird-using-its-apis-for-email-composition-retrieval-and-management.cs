using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path for the email file.
            string emailFilePath = Path.Combine(Directory.GetCurrentDirectory(), "sample.eml");

            // Ensure the directory exists before writing.
            string directoryPath = Path.GetDirectoryName(emailFilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Compose a simple email message.
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Aspose.Email Sample";
                message.Body = "This is a test email created with Aspose.Email.";

                // Save the message to an .eml file.
                try
                {
                    message.Save(emailFilePath, SaveOptions.DefaultEml);
                    Console.WriteLine("Email saved to: " + emailFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save email: " + ex.Message);
                    return;
                }
            }

            // Load the saved email message.
            if (File.Exists(emailFilePath))
            {
                using (MailMessage loadedMessage = MailMessage.Load(emailFilePath))
                {
                    Console.WriteLine("Loaded Email Subject: " + loadedMessage.Subject);
                }
            }
            else
            {
                Console.Error.WriteLine("Email file not found: " + emailFilePath);
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard.
            Console.Error.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
