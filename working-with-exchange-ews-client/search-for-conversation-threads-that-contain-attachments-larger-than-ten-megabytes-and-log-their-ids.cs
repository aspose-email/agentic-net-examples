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
            // Initialize EWS client
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the Inbox folder URI
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // Find all conversations in the Inbox
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderUri);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found.");
                    return;
                }

                const long tenMegabytes = 10L * 1024 * 1024;

                // Iterate through each conversation
                foreach (ExchangeConversation conversation in conversations)
                {
                    bool hasLargeAttachment = false;

                    // Fetch all messages belonging to the conversation
                    MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);
                    if (messages != null)
                    {
                        foreach (MailMessage message in messages)
                        {
                            if (message.Attachments != null)
                            {
                                foreach (Attachment attachment in message.Attachments)
                                {
                                    if (attachment.ContentStream != null && attachment.ContentStream.Length > tenMegabytes)
                                    {
                                        hasLargeAttachment = true;
                                        break;
                                    }
                                }
                            }

                            if (hasLargeAttachment)
                                break;
                        }
                    }

                    // Log conversation ID if a large attachment was found
                    if (hasLargeAttachment)
                    {
                        Console.WriteLine($"Conversation ID with large attachment: {conversation.ConversationId}");
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
