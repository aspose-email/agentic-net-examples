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
            // Development environment guard: skip real network calls if placeholder values are used.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Trust all self‑signed certificates (development only).
            ServicePointManager.ServerCertificateValidationCallback = delegate (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                // Accept self‑signed certificates.
                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                    return true;

                return sslPolicyErrors == SslPolicyErrors.None;
            };

            // Create and use the Exchange WebDav client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    Console.WriteLine($"Total messages in Inbox: {messages.Count}");

                    // Example: display subject of each message (up to 5).
                    int displayed = 0;
                    foreach (var info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        if (++displayed >= 5) break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
