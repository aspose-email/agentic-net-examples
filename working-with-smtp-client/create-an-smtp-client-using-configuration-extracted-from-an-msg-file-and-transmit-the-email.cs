using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file that holds SMTP configuration and the email to send
            string msgFilePath = "email.msg";

            // Ensure the MSG file exists; if not, create a minimal placeholder
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple email message
                    MailMessage placeholderMessage = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "Sample body.");

                    // Add custom headers that will act as SMTP configuration
                    placeholderMessage.Headers.Add("X-SMTP-Host", "smtp.example.com");
                    placeholderMessage.Headers.Add("X-SMTP-Username", "user@example.com");
                    placeholderMessage.Headers.Add("X-SMTP-Password", "password123");

                    // Save the message as MSG
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    placeholderMessage.Save(msgFilePath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MailMessage messageToSend;
            try
            {
                messageToSend = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Extract SMTP configuration from custom headers
            string smtpHost = messageToSend.Headers["X-SMTP-Host"];
            string smtpUsername = messageToSend.Headers["X-SMTP-Username"];
            string smtpPassword = messageToSend.Headers["X-SMTP-Password"];

            if (string.IsNullOrEmpty(smtpHost) ||
                string.IsNullOrEmpty(smtpUsername) ||
                string.IsNullOrEmpty(smtpPassword))
            {
                Console.Error.WriteLine("SMTP configuration headers are missing in the MSG file.");
                return;
            }

            // Create and use the SMTP client
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpUsername, smtpPassword))
                {
                    client.Send(messageToSend);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
