using Aspose.Email.Clients;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Bind the client to a specific local network interface
                client.BindIPEndPoint += remoteEndPoint =>
                    new IPEndPoint(IPAddress.Parse("192.168.1.100"), 0);

                // Build a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(username);
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = "Test Email from Specific Interface";
                    message.Body = "This email was sent using a bound local network interface.";

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
