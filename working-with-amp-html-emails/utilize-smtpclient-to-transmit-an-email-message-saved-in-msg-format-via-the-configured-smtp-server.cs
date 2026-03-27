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
            string msgPath = "message.msg";

            // Verify that the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"The file '{msgPath}' does not exist.");
                return;
            }

            // Load the message from the MSG file.
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Create and configure the SMTP client.
                // Use SSLExplicit to initiate STARTTLS (STARTTLS is represented by SSLExplicit).
                try
                {
                    using (SmtpClient client = new SmtpClient("smtp.example.com", "username", "password", SecurityOptions.SSLExplicit))
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
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
