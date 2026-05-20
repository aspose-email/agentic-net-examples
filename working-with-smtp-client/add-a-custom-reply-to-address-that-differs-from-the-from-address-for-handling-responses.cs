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
            // Prepare output directory and file path
            string outputDir = "Output";
            string emlPath = Path.Combine(outputDir, "sample.eml");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a mail message with custom Reply-To address
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com", "Sender");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Set a Reply-To address that differs from the From address
                message.ReplyToList.Add(new MailAddress("replyto@example.com", "ReplyTo"));

                // Save the message to an EML file
                try
                {
                    message.Save(emlPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Message saved to {emlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }

                // Placeholder SMTP credentials – skip actual sending in CI
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string username = "user@example.com";
                string password = "password";

                if (smtpHost.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder SMTP credentials detected. Skipping send.");
                    return;
                }

                // Send the message using SMTP client
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
