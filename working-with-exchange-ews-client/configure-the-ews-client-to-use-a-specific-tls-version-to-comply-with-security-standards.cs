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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Cast to EmailClient to access TLS settings
                EmailClient emailClient = client as EmailClient;
                if (emailClient != null)
                {
                    // Configure the client to use TLS 1.2 only
                    emailClient.SupportedEncryption = EncryptionProtocols.Tls12;
                }

                // Example operation: retrieve server version info
                try
                {
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine("Exchange Server Version: " + versionInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to get version info: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
