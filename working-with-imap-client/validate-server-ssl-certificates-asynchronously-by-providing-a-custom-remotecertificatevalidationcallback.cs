using Aspose.Email.Clients;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Custom SSL certificate validation callback
            RemoteCertificateValidationCallback certCallback = (object sender,
                                                               X509Certificate certificate,
                                                               X509Chain chain,
                                                               SslPolicyErrors sslPolicyErrors) =>
            {
                // Example: accept all certificates (not for production)
                Console.WriteLine("SSL certificate validation invoked.");
                if (sslPolicyErrors != SslPolicyErrors.None)
                {
                    Console.WriteLine($"SSL policy errors: {sslPolicyErrors}");
                }
                return true;
            };

            // Create and use the ImapClient with the custom callback
            using (ImapClient client = new ImapClient(host, port, username, password, certCallback, SecurityOptions.Auto))
            {
                try
                {
                    // Asynchronously validate credentials (triggers SSL handshake)
                    bool isValid = await client.ValidateCredentialsAsync(null, CancellationToken.None);
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
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
