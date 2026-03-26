using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Replace with your actual EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Get the Inbox folder identifier
                string inboxFolderId = client.MailboxInfo.InboxUri;

                // Find all conversations in the Inbox folder
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);

                // Iterate through each conversation
                foreach (ExchangeConversation conversation in conversations)
                {
                    // Fetch all messages belonging to the current conversation
                    MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);

                    // Iterate through each message and display its subject
                    foreach (MailMessage message in messages)
                    {
                        using (message)
                        {
                            Console.WriteLine("Subject: " + message.Subject);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}