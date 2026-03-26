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
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG-formatted email
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Initialize the SMTP client and configure host, port, and security options
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = "smtp.example.com";
                    client.Port = 587;
                    client.SecurityOptions = SecurityOptions.SSLExplicit; // STARTTLS

                    try
                    {
                        // Send the email
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
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