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
            // Placeholder server URI and credentials.
            string serverUri = "https://exchange.example.com/Exchange";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder values are detected.
            if (serverUri.Contains("example.com") || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder server or credentials detected. Skipping execution.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create and use the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(serverUri, credentials))
            {
                // List messages in the Inbox folder without downloading full content.
                // Each ExchangeMessageInfo provides the Size property.
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    long sizeInBytes = info.Size;
                    Console.WriteLine($"Message URI: {info.UniqueUri}");
                    Console.WriteLine($"Size: {sizeInBytes} bytes");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
