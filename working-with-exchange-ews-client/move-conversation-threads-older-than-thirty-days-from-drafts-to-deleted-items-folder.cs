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
            // Initialize EWS client (replace with actual server URI and credentials)
            string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (ewsUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password", "domain");
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // Get URIs for Drafts and Deleted Items folders
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                string deletedItemsFolderUri = client.MailboxInfo.DeletedItemsUri;

                // Find all conversations in the Drafts folder
                ExchangeConversation[] conversations = client.FindConversations(draftsFolderUri);
                if (conversations == null) return;

                DateTime thresholdDate = DateTime.UtcNow.AddDays(-30);

                foreach (ExchangeConversation conversation in conversations)
                {
                    // Move only conversations whose last delivery time is older than 30 days
                    if (conversation.LastDeliveryTime < thresholdDate)
                    {
                        try
                        {
                            // Move conversation items from Drafts to Deleted Items
                            client.MoveConversationItems(conversation.ConversationId, draftsFolderUri, deletedItemsFolderUri);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to move conversation {conversation.ConversationId}: {ex.Message}");
                        }
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
