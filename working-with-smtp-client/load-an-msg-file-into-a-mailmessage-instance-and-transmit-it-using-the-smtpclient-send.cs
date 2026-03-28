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
            string msgFilePath = "sample.msg";

            // Verify the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MailMessage instance
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Ensure the MailMessage is disposed after use
            using (message)
            {
                // SMTP client configuration (replace with real credentials)
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Create and use the SMTP client
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password))
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
