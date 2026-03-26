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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG‑formatted email
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Ensure the MailMessage is disposed after use
            using (message)
            {
                // SMTP server configuration
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                SecurityOptions security = SecurityOptions.Auto; // Adjust as needed (e.g., SSLImplicit, StartTLS)

                // Create and configure the SmtpClient
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, security))
                {
                    try
                    {
                        // Set authentication credentials (replace with real values)
                        smtpClient.Username = "user@example.com";
                        smtpClient.Password = "password";

                        // Send the email
                        smtpClient.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}