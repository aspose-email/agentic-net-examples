using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration – replace with real values.
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder data is detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping send.");
                return;
            }

            // Configure SOCKS5 proxy.
            string proxyAddress = "127.0.0.1";
            int proxyPort = 1080;
            var proxy = new SocksProxy(proxyAddress, proxyPort, SocksVersion.SocksV5);

            using (var client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                client.Proxy = proxy;

                using (var message = new MailMessage(username, "recipient@example.com", "Test via SOCKS5", "This email is sent through a SOCKS5 proxy."))
                {
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
