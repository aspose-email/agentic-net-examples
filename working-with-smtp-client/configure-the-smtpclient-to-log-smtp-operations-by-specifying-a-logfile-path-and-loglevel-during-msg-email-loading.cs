using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string emlPath = "sample.eml";
            string logPath = "smtp.log";

            // Ensure the EML file exists; create a minimal placeholder if missing
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

                string minimalEml = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test Message\r\n\r\nHello, this is a test email.";
                try
                {
                    File.WriteAllText(emlPath, minimalEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message from the file
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email message: {ex.Message}");
                return;
            }

            // Configure and use SmtpClient with logging
            using (SmtpClient smtpClient = new SmtpClient())
            {
                // Enable logger and specify log file
                smtpClient.EnableLogger = true;
                smtpClient.LogFileName = logPath;

                // Example SMTP server settings (replace with real values as needed)
                smtpClient.Host = "smtp.example.com";
                smtpClient.Port = 587;
                smtpClient.SecurityOptions = SecurityOptions.Auto;
                smtpClient.Username = "user@example.com";
                smtpClient.Password = "password";

                try
                {
                    smtpClient.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (SmtpException smtpEx)
                {
                    Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
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
