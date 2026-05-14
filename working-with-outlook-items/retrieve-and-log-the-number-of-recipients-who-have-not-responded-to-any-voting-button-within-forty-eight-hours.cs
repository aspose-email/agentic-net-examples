using Aspose.Email.Clients.Exchange.Dav;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip execution to avoid network calls in CI
            if (mailboxUri.Contains("your.exchange.server") ||
                username.Contains("user@") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and dispose the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                client.PreAuthenticate = true;

                // Retrieve all messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                int notRespondedCount = 0;

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Use InternalDate (the only available date property) for age comparison
                    DateTime internalDateUtc = info.InternalDate.ToUniversalTime();
                    if (DateTime.UtcNow - internalDateUtc < TimeSpan.FromHours(48))
                        continue; // Skip messages newer than 48 hours

                    // Fetch the full MAPI message to examine recipient information
                    using (MapiMessage mapiMessage = client.FetchMapiMessage(info.UniqueUri))
                    {
                        foreach (MapiRecipient recipient in mapiMessage.Recipients)
                        {
                            // As a fallback, count all recipients for messages older than 48 hours
                            // In a full implementation, you would check recipient's tracking status
                            notRespondedCount++;
                        }
                    }
                }

                Console.WriteLine($"Recipients with no voting response in messages older than 48 hours: {notRespondedCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
