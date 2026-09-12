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
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messagesInfo = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messagesInfo == null || messagesInfo.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                        return;
                    }

                    // Fetch the first message.
                    string messageUri = messagesInfo[0].UniqueUri;
                    MailMessage message = client.FetchMessage(messageUri);

                    // Add a custom extended property as a header.
                    const string customHeaderName = "X-Custom-Property";
                    const string customHeaderValue = "CustomValue";
                    message.Headers.Add(customHeaderName, customHeaderValue);

                    // Display the added header.
                    Console.WriteLine($"Added header: {customHeaderName} = {message.Headers[customHeaderName]}");
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
