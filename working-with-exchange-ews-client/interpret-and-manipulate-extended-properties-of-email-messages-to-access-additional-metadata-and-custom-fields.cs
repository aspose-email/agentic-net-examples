using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messagesInfo = client.ListMessages(client.MailboxInfo.InboxUri);
                if (messagesInfo == null || messagesInfo.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Fetch the first message (including its headers)
                string messageUri = messagesInfo[0].UniqueUri;
                using (MailMessage message = client.FetchMessage(messageUri))
                {
                    // Access a custom extended property via the Headers collection
                    const string customHeader = "X-Custom-Header";
                    string headerValue = message.Headers[customHeader];

                    if (!string.IsNullOrEmpty(headerValue))
                    {
                        Console.WriteLine($"Custom header '{customHeader}': {headerValue}");
                    }
                    else
                    {
                        Console.WriteLine($"Header '{customHeader}' not found in the message.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
