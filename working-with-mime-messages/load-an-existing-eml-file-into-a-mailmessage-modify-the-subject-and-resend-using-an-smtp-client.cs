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
            // Path to the EML file
            string emlFilePath = "input.eml";

            // Ensure the EML file exists
            if (!File.Exists(emlFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"EML file not found: {emlFilePath}");
                try
                {
                    using (FileStream fs = File.Create(emlFilePath))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("From: placeholder@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email.");
                    }
                    Console.WriteLine($"Created placeholder EML file at {emlFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Modify the subject
                message.Subject = "Modified Subject";

                // SMTP server configuration
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUsername = "username";
                string smtpPassword = "password";

                // Skip sending if placeholder credentials are detected
                if (smtpHost.Contains("example.com") || smtpUsername == "username" || smtpPassword == "password")
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                    return;
                }

                // Send the modified message
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
                    {
                        client.Username = smtpUsername;
                        client.Password = smtpPassword;
                        client.SecurityOptions = SecurityOptions.Auto;
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
