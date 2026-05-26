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
            // SMTP server configuration (replace with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // SOCKS proxy configuration (replace with real values)
            string proxyAddress = "proxy.example.com";
            int proxyPort = 1080;
            string proxyUser = "proxyUser";
            string proxyPass = "proxyPass";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping execution.");
                return;
            }

            // Initialize the SMTP client with authentication
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, SecurityOptions.Auto))
            {
                try
                {
                    // Configure SOCKS proxy with authentication
                    client.Proxy = new SocksProxy(proxyAddress, proxyPort, proxyUser, proxyPass);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to configure proxy: {ex.Message}");
                    return;
                }

                // Create a simple email message
                using (MailMessage message = new MailMessage("from@example.com", "to@example.com", "Test Email", "This is a test email sent via SMTP with SOCKS proxy."))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
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
