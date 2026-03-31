using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string domain = "domain";

            // Skip real network call when placeholders are used
            if (ewsUrl.Contains("example.com") ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase) ||
                domain.Equals("domain", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS connection.");
                return;
            }

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password, domain))
            {
                // Enable comprehensive logging
                client.LogFileName = "ews_log.txt";
                client.UseDateInLogFileName = true;

                try
                {
                    // Example operation: retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
