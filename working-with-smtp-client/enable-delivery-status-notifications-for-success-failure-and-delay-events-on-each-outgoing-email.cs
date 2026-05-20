using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
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

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port))
            {
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test email with DSN";
                    message.Body = "This email requests delivery status notifications.";

                    // Enable delivery status notifications for success, failure, and delay
                    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess |
                                                          DeliveryNotificationOptions.OnFailure |
                                                          DeliveryNotificationOptions.Delay;

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
