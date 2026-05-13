using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the body of the email."))
            {
                // Request a read receipt by adding the Disposition-Notification-To header
                message.Headers["Disposition-Notification-To"] = "sender@example.com";

                // Output the headers to verify the addition
                Console.WriteLine("Headers:");
                foreach (var key in message.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {message.Headers[key]}");
                }

                // Normally you would send the message using SmtpClient here
                // SmtpClient client = new SmtpClient("smtp.example.com", 25, "user", "password");
                // client.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
