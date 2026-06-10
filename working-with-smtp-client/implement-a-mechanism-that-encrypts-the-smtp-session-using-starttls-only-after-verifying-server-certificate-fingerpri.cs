using System;
using System.Net.Security;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (placeholders)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";
            string expectedFingerprint = "AB:CD:EF:12:34:56:78:90:AB:CD:EF:12:34:56:78:90:AB:CD:EF:12:34:56:78:90:AB:CD:EF:12:34:56:78:90";

            // Skip real network call when using placeholder data
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping actual send.");
                return;
            }

            // Certificate validation callback that checks the SHA‑256 fingerprint
            RemoteCertificateValidationCallback certCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (certificate == null)
                    return false;

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(certificate.GetRawCertData());
                    string fingerprint = BitConverter.ToString(hash).Replace("-", ":");
                    return string.Equals(fingerprint, expectedFingerprint, StringComparison.OrdinalIgnoreCase);
                }
            };

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, certCallback))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.SSLExplicit; // STARTTLS

                    // Create the email message
                    using (MailMessage message = new MailMessage(
                        "from@example.com",
                        "to@example.com",
                        "Test Subject",
                        "This is a test email sent with STARTTLS after certificate fingerprint verification."))
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
