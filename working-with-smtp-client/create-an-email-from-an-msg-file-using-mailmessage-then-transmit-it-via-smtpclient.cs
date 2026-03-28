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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // SMTP server configuration (replace with real values)
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Create and configure the SMTP client
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password))
                    {
                        // Send the message
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
