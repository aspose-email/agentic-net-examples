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
            // Placeholder connection details
            string host = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create and dispose the Exchange client safely
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                try
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Iterate over each message and count its attachments
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        int attachmentCount = messageInfo.Attachments?.Count ?? 0;
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"Attachment count: {attachmentCount}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
