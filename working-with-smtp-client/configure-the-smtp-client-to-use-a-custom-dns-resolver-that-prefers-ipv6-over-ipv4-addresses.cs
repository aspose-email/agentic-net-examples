using Aspose.Email.Clients;
using System;
using System.Linq;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (smtpHost.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping execution.");
                return;
            }

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password, SecurityOptions.SSLExplicit))
            {
                // Resolve the host manually, preferring IPv6 addresses.
                try
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(smtpHost);
                    IPAddress selected = addresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                                         ?? addresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    if (selected != null)
                    {
                        client.Host = selected.ToString(); // Use the resolved IP address.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"DNS resolution failed: {ex.Message}");
                    return;
                }

                // Validate credentials safely.
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP validation failed: {ex.Message}");
                    return;
                }

                // Create a simple email message.
                MailMessage message = new MailMessage();
                message.From = new MailAddress(username);
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test email";
                message.Body = "This is a test.";

                // Send the message.
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
