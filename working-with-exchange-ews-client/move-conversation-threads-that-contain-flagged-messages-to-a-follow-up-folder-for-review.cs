using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with actual values)
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Use IEWSClient via EWSClient.GetEWSClient
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Ensure the destination folder "Follow Up" exists under the Inbox
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeFolderInfo followUpFolderInfo = null;
                    bool followUpExists = client.FolderExists(inboxUri, "Follow Up", out followUpFolderInfo);
                    if (!followUpExists)
                    {
                        // Create the folder under the Inbox
                        ExchangeFolderInfo createdFolder = client.CreateFolder(inboxUri, "Follow Up");
                        followUpFolderInfo = createdFolder;
                    }

                    // Find all conversations in the Inbox
                    ExchangeConversation[] conversations = client.FindConversations(inboxUri);
                    foreach (ExchangeConversation conversation in conversations)
                    {
                        // Check if the conversation contains flagged items
                        if (conversation.FlagStatus == ExchangeConversationFlagStatus.Flagged)
                        {
                            // Move all items of this conversation to the "Follow Up" folder
                            client.MoveConversationItems(conversation.ConversationId, followUpFolderInfo.Uri);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
