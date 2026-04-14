using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Enable response decompression (available property)
                client.EnableDecompression = true;

                // Add request header to ask server to compress responses (gzip)
                client.AddHeader("Accept-Encoding", "gzip");

                // Example operation: fetch mailbox info
                ExchangeMailboxInfo info = client.MailboxInfo;
                Console.WriteLine($"Display name: {info.MailboxUri}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
