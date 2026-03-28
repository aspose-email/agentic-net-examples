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
            // Path to the MSG file to be sent
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
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
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create and configure the SMTP client
            // Replace host, port, username, and password with real values as needed
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = "smtp.example.com";
                client.Port = 465; // Implicit SSL port
                client.Username = "username";
                client.Password = "password";

                // Enable SSL for the connection
                client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Send the message inside a protected block
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
