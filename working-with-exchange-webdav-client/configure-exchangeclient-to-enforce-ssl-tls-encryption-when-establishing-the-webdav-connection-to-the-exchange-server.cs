using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Enforce TLS/SSL encryption protocols globally for the connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            // Create the ExchangeClient inside a using block to ensure proper disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Optional: verify connection by attempting to list messages in the Inbox
                try
                {
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, 5);
                    Console.WriteLine($"Successfully retrieved {messages.Count} messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection or authentication failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
