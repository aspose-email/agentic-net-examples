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
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List all messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Fetch the full MailMessage for each item
                        using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                        {
                            // Process each message (example: output subject)
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during message processing: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
