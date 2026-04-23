using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real credentials in production
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string proxyHost = "proxy.corporate.com";
            int proxyPort = 8080;

            // Skip execution when placeholder data is detected to avoid unwanted network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange client execution.");
                return;
            }

            // Configure the HTTP proxy
            WebProxy httpProxy = new WebProxy(proxyHost, proxyPort);
            // If the proxy requires authentication, set credentials here:
            // httpProxy.Credentials = new NetworkCredential("proxyUser", "proxyPass");

            // Create and configure the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                client.Proxy = httpProxy;

                // Example operation: retrieve mailbox information safely
                try
                {
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to retrieve mailbox info: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
