using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

namespace GmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace with your Gmail credentials
            string username = "your.email@gmail.com";
            string password = "your_app_password";

            // Create the SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587, username, password);
            client.SecurityOptions = SecurityOptions.Auto;

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = username;
            message.To = "recipient@example.com";
            message.Subject = "Test email from Aspose.Email";
            message.Body = "Hello, this is a test email sent using Aspose.Email library.";

            try
            {
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
