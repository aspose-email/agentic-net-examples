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
            // SMTP server configuration
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Initialize the SMTP client (variable name must be 'client')
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                // Enable communication logging
                client.EnableLogger = true;
                client.LogFileName = "smtp_log.txt";

                // Path to the MSG file to be sent
                string msgPath = "sample.msg";

                // Verify that the MSG file exists
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the message from the MSG file
                MailMessage message;
                try
                {
                    message = MailMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                    return;
                }

                // Ensure the MailMessage is disposed after use
                using (message)
                {
                    try
                    {
                        // Send the message
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
