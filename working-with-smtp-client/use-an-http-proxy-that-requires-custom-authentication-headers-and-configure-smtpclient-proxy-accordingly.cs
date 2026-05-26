using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Early exit if placeholder values are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping execution.");
                return;
            }

            // Proxy configuration (requires authentication)
            string proxyAddress = "proxy.mycompany.com";
            int proxyPort = 8080;
            string proxyUser = "proxyUser";
            string proxyPass = "proxyPass";

            // Create the HTTP proxy with authentication credentials
            HttpProxy proxy = new HttpProxy(proxyAddress, proxyPort, proxyUser, proxyPass);

            // Create the SMTP client with explicit TLS (STARTTLS)
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, SecurityOptions.SSLExplicit))
            {
                client.Proxy = proxy;

                // Build a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = smtpUser;
                    message.To.Add("recipient@domain.com");
                    message.Subject = "Test Email via Proxy";
                    message.Body = "This email was sent using Aspose.Email with a custom authenticated HTTP proxy.";

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
