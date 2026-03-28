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
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Identify the folder to search (Inbox)
                string folderId = client.MailboxInfo.InboxUri;

                // Find conversations in the specified folder
                ExchangeConversation[] conversations = client.FindConversations(folderId);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found in the folder.");
                    return;
                }

                // Select a conversation (e.g., the first one)
                string conversationId = conversations[0].ConversationId;

                // Retrieve all messages belonging to the selected conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);
                foreach (MailMessage message in messages)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
