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
            // Define SMTP server settings
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Path to the MSG file to be sent
            string msgPath = "email.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the email message from the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, SecurityOptions.Auto))
                {
                    client.Username = smtpUser;
                    client.Password = smtpPass;

                    try
                    {
                        // Send the loaded message
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
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