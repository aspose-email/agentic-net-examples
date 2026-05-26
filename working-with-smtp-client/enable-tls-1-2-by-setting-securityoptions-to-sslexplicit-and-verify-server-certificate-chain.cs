using Aspose.Email.Clients;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create a MailMessage (sample content)
            using (MailMessage message = new MailMessage())
            {
                message.From = username;
                message.To.Add(username);
                message.Subject = "Test Email";
                message.Body = "This is a test email sent using Aspose.Email with TLS 1.2.";

                // Create SmtpClient with TLS 1.2 (SSLExplicit) and certificate validation callback
                using (SmtpClient smtpClient = new SmtpClient(host, port, username, password, ServerCertificateValidationCallback))
                {
                    smtpClient.SecurityOptions = SecurityOptions.SSLExplicit;

                    try
                    {
                        // Validate credentials before sending
                        smtpClient.ValidateCredentials();

                        // Send the email
                        smtpClient.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    // Callback to verify the server certificate chain
    private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // If there are no SSL policy errors, the certificate is valid
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        // Additional custom validation can be added here
        Console.Error.WriteLine($"Certificate error: {sslPolicyErrors}");
        return false;
    }
}
