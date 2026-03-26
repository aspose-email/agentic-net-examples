using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
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

            // Load the MSG-formatted email
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // SMTP server configuration
                string smtpHost = "smtp.example.com";
                int smtpPort = 587; // Common port for STARTTLS

                // Create and configure the SmtpClient
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = smtpHost;
                    client.Port = smtpPort;
                    // Use STARTTLS (explicit TLS) security mode
                    client.SecurityOptions = SecurityOptions.SSLExplicit;

                    try
                    {
                        // Send the email
                        client.Send(message);
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