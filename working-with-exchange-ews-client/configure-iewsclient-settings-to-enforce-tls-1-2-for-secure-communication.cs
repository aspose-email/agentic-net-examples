using Aspose.Email.Clients.Base;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Enforce TLS 1.2 for the underlying EmailClient
                    EmailClient emailClient = client as EmailClient;
                    if (emailClient != null)
                    {
                        emailClient.SupportedEncryption = EncryptionProtocols.Tls12;
                    }

                    // Example operation: retrieve server version
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine("Exchange Server version: " + versionInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
        }
    }
}
