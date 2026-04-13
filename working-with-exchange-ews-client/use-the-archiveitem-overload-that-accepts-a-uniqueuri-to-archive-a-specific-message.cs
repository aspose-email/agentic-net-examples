using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Get the Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                        return;
                    }

                    // Take the first message and obtain its unique identifier (UniqueUri)
                    ExchangeMessageInfo firstMessage = messages[0];
                    string uniqueId = firstMessage.UniqueUri;

                    // Archive the specific message using the overload that accepts a unique ID
                    client.ArchiveItem(inboxUri, uniqueId);
                    Console.WriteLine("Message archived successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create or connect EWS client: {ex.Message}");
        }
    }
}
