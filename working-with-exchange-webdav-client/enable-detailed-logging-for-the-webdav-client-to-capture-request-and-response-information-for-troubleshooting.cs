using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details
                string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
                string username = "username";
                string password = "password";

                // Skip real network call when placeholders are used
                if (mailboxUri.Contains("example.com") || username == "username")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping live connection.");
                    return;
                }

                // Initialize the WebDAV client
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Enable detailed logging by specifying a log file
                    client.LogFileName = "exchange_client.log";
                    client.UseDateInLogFileName = true; // optional, adds date to log file name

                    // Example operation to generate log entries
                    try
                    {
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        Console.WriteLine($"Retrieved {messages.Count} messages from Inbox.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
