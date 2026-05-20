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
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";
            string localIp = "192.168.1.100";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Bind the client to a specific local IP address
                client.BindIPEndPoint += (remoteEndPoint) =>
                {
                    return new IPEndPoint(IPAddress.Parse(localIp), 0);
                };

                // Create a simple email message
                MailMessage message = new MailMessage("from@example.com", "to@example.com", "Test Subject", "Test body");
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (SmtpException ex)
                {
                    Console.Error.WriteLine($"SMTP error: {ex.Message}");
                }
                finally
                {
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
