using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create and connect the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Get the Sent Items folder URI
                    string sentFolderUri = client.MailboxInfo.SentItemsUri;

                    // Find all conversations in Sent Items
                    ExchangeConversation[] conversations = client.FindConversations(sentFolderUri);

                    // Define the subject pattern to match
                    const string subjectPattern = "SpecificPattern";

                    foreach (ExchangeConversation conversation in conversations)
                    {
                        // Check if the conversation topic matches the pattern
                        if (!string.IsNullOrEmpty(conversation.ConversationTopic) &&
                            conversation.ConversationTopic.Contains(subjectPattern, StringComparison.OrdinalIgnoreCase))
                        {
                            // Delete all items of the matching conversation from Sent Items
                            client.DeleteConversationItems(conversation.ConversationId, sentFolderUri);
                            Console.WriteLine($"Deleted conversation with ID: {conversation.ConversationId}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing conversations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create or connect client: {ex.Message}");
        }
    }
}
