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
            // Configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPassword = "password";
            string msgFilePath = "email.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgFilePath))
            {
                // Create and configure the SMTP client
                using (Aspose.Email.Clients.Smtp.SmtpClient smtpClient = new Aspose.Email.Clients.Smtp.SmtpClient(
                    smtpHost,
                    smtpPort,
                    smtpUser,
                    smtpPassword,
                    SecurityOptions.Auto))
                {
                    try
                    {
                        // Send the message
                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception sendEx)
                    {
                        Console.Error.WriteLine($"Error sending email: {sendEx.Message}");
                        return;
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