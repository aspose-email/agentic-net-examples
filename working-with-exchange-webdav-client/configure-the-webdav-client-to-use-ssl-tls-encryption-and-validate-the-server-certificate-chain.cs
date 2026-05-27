using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder mailbox URI – skip actual connection when using placeholders
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder mailbox URI detected. Skipping connection.");
                return;
            }

            string username = "user@example.com";
            string password = "password";

            // Enforce TLS 1.2 (or higher) for all outgoing requests
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Validate the server certificate chain before any request
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                Console.Error.WriteLine("Server certificate validation failed.");
                return false;
            };

            // Create the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Load a client certificate if a file is provided
                string clientCertPath = "client.pfx";
                if (File.Exists(clientCertPath))
                {
                    try
                    {
                        X509Certificate2 clientCertificate = new X509Certificate2(clientCertPath, "certPassword");
                        client.ClientCertificate = clientCertificate;
                    }
                    catch (Exception certEx)
                    {
                        Console.Error.WriteLine($"Failed to load client certificate: {certEx.Message}");
                    }
                }

                // Perform a safe operation to verify connectivity
                try
                {
                    // ListMessages returns ExchangeMessageInfoCollection
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, 1);
                    Console.WriteLine($"Retrieved {messages.Count} message(s) from the inbox.");

                    foreach (var msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception opEx)
                {
                    Console.Error.WriteLine($"Operation failed: {opEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
