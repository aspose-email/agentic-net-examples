using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Identify the folder to search (Inbox)
                    string inboxFolderId = client.MailboxInfo.InboxUri;

                    // Find all conversations in the Inbox
                    ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);

                    if (conversations == null || conversations.Length == 0)
                    {
                        Console.WriteLine("No conversations found in the Inbox.");
                        return;
                    }

                    // Placeholder conversation identifier (use the first conversation's ID if not set)
                    string targetConversationId = conversations[0].ConversationId;

                    // Fetch all messages belonging to the specified conversation
                    MailMessageCollection messages = client.FetchConversationMessages(targetConversationId);

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found for the conversation.");
                        return;
                    }

                    // Output basic information about each message
                    foreach (MailMessage message in messages)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
