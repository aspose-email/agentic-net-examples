using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (exchangeUrl.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create the Exchange client (WebDAV)
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                // Define a size filter: messages larger than 5 MB (5 * 1024 * 1024 = 5242880 bytes)
                const long fiveMegabytes = 5L * 1024L * 1024L;
                string sizeQuery = $"size>{fiveMegabytes}";

                // List messages from the Inbox that satisfy the size filter
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, sizeQuery);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"Size (bytes): {messageInfo.Size}");
                    Console.WriteLine($"URI: {messageInfo.UniqueUri}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
