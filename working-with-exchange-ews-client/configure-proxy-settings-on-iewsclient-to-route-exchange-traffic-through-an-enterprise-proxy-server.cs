using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace ProxyConfigurationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define Exchange server mailbox URI and user credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Configure enterprise proxy settings
                WebProxy proxy = new WebProxy
                {
                    Address = new Uri("http://proxy.enterprise.com:8080"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("proxyUser", "proxyPassword")
                };

                // Create IEWSClient instance with proxy
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, proxy))
                {
                    try
                    {
                        // Retrieve mailbox information
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                        // Output some mailbox URIs to verify connection
                        Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    }
                    catch (ExchangeException ex)
                    {
                        Console.Error.WriteLine("Exchange operation failed: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
