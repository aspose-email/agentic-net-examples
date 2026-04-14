using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace EwsLoggingSample
{
    // Custom proxy that logs request URIs.
    class LoggingWebProxy : IWebProxy
    {
        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            Console.WriteLine($"[EWS Request] Destination: {destination}");
            // Return the original destination without modification.
            return destination;
        }

        public bool IsBypassed(Uri host)
        {
            // Do not bypass any hosts; log all.
            return false;
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare connection parameters.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client with a logging proxy.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Attach the custom HTTP handler (proxy) to log requests.
                    client.Proxy = new LoggingWebProxy();

                    // Optional: also log responses by handling the client’s LogFileName.
                    // This writes raw request/response data to the specified file.
                    client.LogFileName = "EwsLog.txt";

                    // Perform a simple operation to generate traffic.
                    try
                    {
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Mailbox display name: {mailboxInfo.MailboxUri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
