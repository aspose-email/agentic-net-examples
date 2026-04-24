using Aspose.Email.Clients.Base;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // SMTP server configuration (replace with real credentials)
            string host = "smtp.gmail.com";
            int port = 587;
            string username = "your.email@gmail.com";
            string password = "yourpassword";

            // Guard against placeholder credentials to avoid external calls during CI
            if (string.IsNullOrEmpty(host) || host.Contains("example") ||
                string.IsNullOrEmpty(username) || username.Contains("example") ||
                string.IsNullOrEmpty(password) || password.Contains("example"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping SMTP connection.");
                return;
            }

            // Certificate validation callback
            RemoteCertificateValidationCallback certificateCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                // Examine each chain status element
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    if (status.Status != X509ChainStatusFlags.NoError)
                    {
                        Console.Error.WriteLine($"Certificate validation error: {status.StatusInformation}");
                        return false;
                    }
                }

                // If we reach here, treat the certificate as valid
                return true;
            };

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, certificateCallback))
            {
                // Enforce TLS 1.2 only
                client.SupportedEncryption = EncryptionProtocols.Tls12;

                try
                {
                    // Validate credentials (establishes a connection and triggers TLS handshake)
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine($"Credentials validation result: {isValid}");
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
