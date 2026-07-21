using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    // Author: Aspose.Email example generator
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip network call when placeholders are detected.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("example.com") ||
                username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client safely.
            IEWSClient ewsClient = null;
            try
            {
                ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure proper disposal.
            using (ewsClient as IDisposable)
            {
                try
                {
                    // Retrieve mailbox information.
                    ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();

                    // List messages from the Inbox folder.
                    ExchangeMessageInfoCollection messages = ewsClient.ListMessages(mailboxInfo.InboxUri);

                    Console.WriteLine($"Found {messages.Count} messages in Inbox:");
                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        Console.WriteLine($"- Subject: {msgInfo.Subject}");
                    }
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
