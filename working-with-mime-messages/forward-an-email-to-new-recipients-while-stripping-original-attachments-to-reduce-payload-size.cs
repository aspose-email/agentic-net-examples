using Aspose.Email.Clients;
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
            // Input EML file path
            string emlPath = "input.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    MailMessage placeholder = new MailMessage();
                    placeholder.From = new MailAddress("placeholder@example.com");
                    placeholder.To.Add(new MailAddress("recipient@example.com"));
                    placeholder.Subject = "Placeholder Email";
                    placeholder.Body = "This is a placeholder email.";
                    placeholder.Save(emlPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the original message
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Remove all attachments to reduce payload size
            originalMessage.Attachments.Clear();

            // SMTP server configuration (placeholder values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against executing with placeholder credentials
            if (smtpHost.Contains("example.com"))
            {
                Console.WriteLine("SMTP host is a placeholder. Skipping send operation.");
                return;
            }

            // Define sender and new recipient(s)
            string senderAddress = "sender@example.com";
            string recipientAddress = "newrecipient@example.com";

            // Send (forward) the message
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Username = smtpUsername;
                    smtpClient.Password = smtpPassword;
                    smtpClient.SecurityOptions = SecurityOptions.Auto;

                    // Forward the message without original attachments
                    smtpClient.Forward(senderAddress, recipientAddress, originalMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to forward email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
