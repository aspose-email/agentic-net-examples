using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace ProgrammingWithGmail
{
    class Program
    {
        static void Main(string[] args)
        {
            // Subscription service email address (replace with actual address if known)
            const string subscriptionEmail = "subscribe@example.com";

            // Create the subscription request email
            var message = new MailMessage
            {
                From = "your.email@gmail.com",          // Replace with your Gmail address
                To = subscriptionEmail,
                Subject = "Subscribe to product updates",
                Body = "Please add me to the product update notification list."
            };

            // Configure Gmail SMTP client (use your credentials)
            var client = new SmtpClient("smtp.gmail.com", 587, "your.email@gmail.com", "your_password")
            {
                SecurityOptions = SecurityOptions.Auto
            };

            try
            {
                client.Send(message);
                Console.WriteLine("Subscription request sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send subscription request: {ex.Message}");
            }
        }
    }
}
