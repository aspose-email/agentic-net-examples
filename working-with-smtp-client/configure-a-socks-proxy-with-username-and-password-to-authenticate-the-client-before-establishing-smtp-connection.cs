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
            // SMTP server settings (replace with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // SOCKS proxy settings (replace with real values)
            string proxyHost = "proxy.example.com";
            int proxyPort = 1080;
            string proxyUser = "proxyUser";
            string proxyPass = "proxyPass";

            // Detect placeholder values and skip real network calls
            if (smtpHost.Contains("example.com") || proxyHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping SMTP operation.");
                return;
            }

            // Create and configure the SMTP client inside a using block
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
            {
                // Optional: set security options (Auto will negotiate TLS if needed)
                client.SecurityOptions = SecurityOptions.Auto;

                // Configure SOCKS5 proxy with authentication
                client.Proxy = new SocksProxy(proxyHost, proxyPort, proxyUser, proxyPass);

                try
                {
                    // Validate credentials before sending
                    if (!client.ValidateCredentials())
                    {
                        Console.Error.WriteLine("SMTP authentication failed.");
                        return;
                    }

                    // Create a simple email message
                    using (MailMessage message = new MailMessage(smtpUser, "recipient@example.com", "Test Email", "This is a test email sent via SMTP with SOCKS proxy."))
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
