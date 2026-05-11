using Aspose.Email.Clients.Base;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Enforce TLS 1.2 for secure communication
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    Console.WriteLine("TLS 1.2 has been enforced for the EWS client.");

                    // Optional: verify connection by retrieving mailbox info
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine($"Connected to mailbox: {mailboxInfo.MailboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS client operation failed: {ex.Message}");
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
