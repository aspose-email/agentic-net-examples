using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if they are not replaced with real values.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping server operations.");
                return;
            }

            // Create the EWS client inside a try/catch to handle connection/authentication errors.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Get the Inbox folder URI.
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // Retrieve messages from the Inbox.
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // ----- Process the message here -----
                        // Example: display subject (replace with actual processing logic).
                        Console.WriteLine($"Processing message: {messageInfo.Subject}");

                        // After processing, mark the message as read on the server.
                        // Use UniqueUri as the identifier (ExchangeMessageInfo.Uri is not available).
                        client.SetReadFlag(messageInfo.UniqueUri);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
