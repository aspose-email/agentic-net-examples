using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths and SMTP parameters (placeholders)
            string msgFilePath = "sample.msg";
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "username";
            string smtpPassword = "password";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholderMsg = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholderMsg.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and convert to MailMessage
            MailMessage mailMessage;
            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                {
                    mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or convert MSG file: {ex.Message}");
                return;
            }

            // Guard against placeholder SMTP credentials
            if (smtpHost.Contains("example.com") || smtpUsername.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                return;
            }

            // Send the email via SMTP
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword))
                {
                    client.Send(mailMessage);
                    Console.WriteLine("Message sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose MailMessage explicitly
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
