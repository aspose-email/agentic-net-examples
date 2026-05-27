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
            // SMTP server configuration (replace with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPassword = "password";

            // HTTP proxy configuration with NTLM authentication (replace with real values)
            string proxyHost = "proxy.example.com";
            int proxyPort = 8080;
            string proxyUser = "proxyUser";
            string proxyPassword = "proxyPass";
            string proxyDomain = "DOMAIN";

            // Skip execution when placeholder values are detected to avoid unwanted network calls
            if (smtpHost.Contains("example") || proxyHost.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping email send.");
                return;
            }

            // Initialize the SMTP client
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword))
            {
                // Configure NTLM proxy authentication
                client.UseDefaultCredentials = false;
                var httpProxy = new HttpProxy(proxyHost, proxyPort);
                httpProxy.Credentials = new NetworkCredential(proxyUser, proxyPassword, proxyDomain);
                client.Proxy = httpProxy;

                // Create the email message
                MailMessage message = new MailMessage();
                message.From = smtpUser;
                message.To.Add("recipient@example.com");
                message.Subject = "Test email via NTLM proxy";
                message.Body = "This email was sent using Aspose.Email with an NTLM authenticated HTTP proxy.";

                // Send the message
                client.Send(message);
                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
