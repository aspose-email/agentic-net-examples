using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSmtpProxyExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP server details
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUsername = "user@example.com";
                string smtpPassword = "password";

                // Detect placeholder credentials and skip actual network call
                if (smtpHost.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping actual send.");
                    return;
                }

                // Configure HTTP proxy
                var httpProxy = new HttpProxy("proxy.example.com", 8080);

                // Initialize SmtpClient with explicit parameters
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword, SecurityOptions.Auto))
                {
                    // Assign the proxy to the client
                    client.Proxy = httpProxy;

                    // Create a simple mail message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpUsername);
                        message.To.Add(new MailAddress("recipient@example.com"));
                        message.Subject = "Test Email via Proxy";
                        message.Body = "This email was sent using Aspose.Email with an HTTP proxy.";

                        try
                        {
                            // Send the message
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception sendEx)
                        {
                            Console.Error.WriteLine($"Error sending email: {sendEx.Message}");
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
}
