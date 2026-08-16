using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Test Email via Loopback SMTP";
            message.Body = "This is a test email sent to a local SMTP server.";

            // Bind the SMTP client to the loopback address (localhost) on port 25
            SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25);

            // Optionally set security options if needed (none for plain loopback)
            // smtpClient.SecurityOptions = SecurityOptions.Auto;

            // Send the message
            using (smtpClient)
            {
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully to the loopback SMTP server.");
            }
        }
        catch (SmtpException ex)
        {
            Console.Error.WriteLine($"SMTP error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
