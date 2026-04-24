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
            // Placeholder credentials – skip actual network call in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping IMAP connection.");
                return;
            }

            // Callback that ignores all certificate validation errors (testing only)
            RemoteCertificateValidationCallback ignoreCertErrors = delegate (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                return true; // Accept any certificate
            };

            // Create and configure the ImapClient
            using (ImapClient client = new ImapClient(host, port, username, password, ignoreCertErrors))
            {
                try
                {
                    // Perform a lightweight operation to validate the connection
                    client.SelectFolder("INBOX");
                    Console.WriteLine("Connected to IMAP server and selected INBOX successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
