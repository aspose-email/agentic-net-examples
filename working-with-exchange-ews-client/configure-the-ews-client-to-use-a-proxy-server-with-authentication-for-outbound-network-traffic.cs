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
            // Exchange server mailbox URI and user credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "userPassword";

            // Proxy server settings with authentication
            string proxyUri = "http://proxy.example.com:8080";
            string proxyUser = "proxyUser";
            string proxyPassword = "proxyPass";

            // Configure the proxy
            WebProxy proxy = new WebProxy(proxyUri)
            {
                Credentials = new NetworkCredential(proxyUser, proxyPassword)
            };

            // Create the EWS client using the overload that accepts a proxy
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, proxy))
            {
                // Example operation: display the Inbox folder URI
                Console.WriteLine("Inbox URI: " + client.MailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
