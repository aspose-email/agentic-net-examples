using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file containing the email to be sent
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the message from the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Placeholder SMTP configuration – replace with actual values or extract from the message if available
                string host = "smtp.example.com";
                string username = "user@example.com";
                string password = "password";

                // Create and use the SMTP client to send the message
                try
                {
                    using (SmtpClient client = new SmtpClient(host, username, password))
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
