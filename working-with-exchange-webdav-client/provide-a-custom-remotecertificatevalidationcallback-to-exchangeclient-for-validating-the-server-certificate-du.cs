using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Assign custom certificate validation callback
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Example operation: list first 5 messages from Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, 5);
                    Console.WriteLine($"Fetched {messages.Count} messages from Inbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    // Custom RemoteCertificateValidationCallback
    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        Console.Error.WriteLine($"SSL certificate validation error: {sslPolicyErrors}");
        // Additional inspection of the certificate can be added here
        return false;
    }
}
