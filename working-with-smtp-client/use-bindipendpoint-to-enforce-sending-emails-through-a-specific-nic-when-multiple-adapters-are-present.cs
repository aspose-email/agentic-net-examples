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
            // SMTP server configuration (replace placeholders with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the SMTP client
            SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto);

            // Bind the client to a specific local IP address (NIC)
            client.BindIPEndPoint += remoteEndPoint =>
            {
                // Specify the local IP address to use
                IPAddress localIp = IPAddress.Parse("192.168.1.100");
                // Port 0 lets the OS select an available local port
                return new IPEndPoint(localIp, 0);
            };

            // Build the email message
            MailMessage message = new MailMessage
            {
                From = new MailAddress(username),
                Subject = "Test email with specific NIC",
                Body = "This email is sent using a specific network interface."
            };
            message.To.Add(new MailAddress("recipient@example.com"));

            // Send the email
            client.Send(message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
