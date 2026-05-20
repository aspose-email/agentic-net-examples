using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual send when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    // Create the email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@domain.com";
                        message.To.Add("recipient@domain.com");
                        message.Subject = "Urgent Notification";
                        message.Body = "This is a time‑sensitive notification.";

                        // Add custom X-Priority-Level header
                        message.Headers.Add("X-Priority-Level", "urgent");

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
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
