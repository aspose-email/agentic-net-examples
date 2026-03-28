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
            // Set up credentials and service URL
            NetworkCredential credential = new NetworkCredential("username", "password");
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Placeholder: use a property that identifies the conversation.
                    // In real scenarios, replace with the appropriate ConversationId property.
                    string conversationId = info.UniqueUri;

                    // Retrieve all messages belonging to the conversation
                    MailMessageCollection conversationMessages = client.FetchConversationMessages(conversationId);

                    Console.WriteLine($"Conversation ID: {conversationId}");
                    Console.WriteLine($"Number of messages in conversation: {conversationMessages.Count}");

                    foreach (MailMessage message in conversationMessages)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine("---");
                    }

                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
