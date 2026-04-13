using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client (WebDAV)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the default Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages();

                // Log subject and sender for each message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"Sender: {info.From}");
                    Console.WriteLine("---");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
