using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Certificate validation callback
            RemoteCertificateValidationCallback certCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    Console.WriteLine("Server certificate is valid.");
                    return true;
                }
                else
                {
                    Console.Error.WriteLine($"Certificate validation error: {sslPolicyErrors}");
                    return false;
                }
            };

            // Create and use the IMAP client with SSL/TLS
            using (ImapClient client = new ImapClient(host, port, username, password, certCallback, SecurityOptions.SSLImplicit))
            {
                try
                {
                    bool credentialsValid = client.ValidateCredentials();
                    Console.WriteLine($"Credentials validation result: {credentialsValid}");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
