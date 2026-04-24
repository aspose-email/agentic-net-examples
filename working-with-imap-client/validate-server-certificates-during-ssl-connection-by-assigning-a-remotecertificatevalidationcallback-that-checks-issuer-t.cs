using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when using placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping connection.");
                return;
            }

            // Certificate validation callback that trusts certificates issued by a specific issuer
            RemoteCertificateValidationCallback certCallback = delegate (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                if (chain == null || chain.ChainElements.Count == 0)
                {
                    Console.Error.WriteLine("Certificate chain is unavailable.");
                    return false;
                }

                // Example: trust certificates whose root issuer contains "TrustedIssuer"
                X509Certificate2 rootCert = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                bool isTrustedIssuer = rootCert.Issuer.Contains("TrustedIssuer", StringComparison.OrdinalIgnoreCase);

                if (!isTrustedIssuer)
                {
                    Console.Error.WriteLine($"Untrusted certificate issuer: {rootCert.Issuer}");
                }

                return isTrustedIssuer;
            };

            // Create and use the ImapClient with the custom certificate validation callback
            using (ImapClient client = new ImapClient(host, port, username, password, certCallback))
            {
                try
                {
                    // Attempt to validate credentials (lightweight operation)
                    bool credentialsValid = client.ValidateCredentials();
                    Console.WriteLine($"Credentials valid: {credentialsValid}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
