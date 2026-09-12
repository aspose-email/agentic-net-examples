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
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create and connect the Exchange client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Retrieve up to ten messages from the Inbox.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, 10);

                    long totalSize = 0;
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        totalSize += info.Size; // Size is in bytes.
                    }

                    Console.WriteLine($"Estimated total download size for {messages.Count} messages: {totalSize} bytes.");

                    // Example of using UniqueUri for later fetching (not executed here).
                    // foreach (ExchangeMessageInfo info in messages)
                    // {
                    //     string uri = info.UniqueUri;
                    //     // MailMessage fullMessage = client.FetchMessage(uri);
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
