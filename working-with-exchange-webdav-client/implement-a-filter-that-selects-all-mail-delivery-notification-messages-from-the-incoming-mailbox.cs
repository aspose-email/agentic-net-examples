using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – real credentials should be supplied in production.
            string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder values are detected.
            if (serverUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping Exchange server call.");
                return;
            }

            // Create and use the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
            {
                try
                {
                    // Retrieve all messages from the Inbox folder.
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate through the messages and select delivery notifications.
                    foreach (ExchangeMessageInfo info in messageInfos)
                    {
                        // Delivery notifications typically have a MessageClass that starts with "REPORT.IPM".
                        if (info.MessageClass != null &&
                            info.MessageClass.StartsWith("REPORT.IPM", StringComparison.OrdinalIgnoreCase))
                        {
                            // Fetch the full message to inspect or process it further.
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                Console.WriteLine($"Delivery Notification - Subject: {message.Subject}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle client‑related errors (connection, authentication, etc.).
                    Console.Error.WriteLine($"Exchange client error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level guard to prevent unhandled exceptions.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
