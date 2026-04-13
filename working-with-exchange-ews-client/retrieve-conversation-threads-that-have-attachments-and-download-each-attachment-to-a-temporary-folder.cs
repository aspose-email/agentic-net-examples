using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a temporary folder for attachments
            string tempFolder = Path.Combine(Path.GetTempPath(), "EwsAttachments");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            // Create the EWS client (replace with actual server URI and credentials)
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
                // Get the Inbox folder identifier
                string inboxFolderId = client.MailboxInfo.InboxUri;

                // Find all conversations in the Inbox
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);
                foreach (ExchangeConversation conversation in conversations)
                {
                    // Process only conversations that have attachments
                    if (conversation.HasAttachments)
                    {
                        // Retrieve all messages belonging to the conversation
                        MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);
                        foreach (MailMessage message in messages)
                        {
                            // Iterate through each attachment in the message
                            foreach (Attachment attachment in message.Attachments)
                            {
                                // Build a unique file path for the attachment
                                string filePath = Path.Combine(tempFolder, attachment.Name);
                                int duplicateIndex = 1;
                                while (File.Exists(filePath))
                                {
                                    string nameWithoutExt = Path.GetFileNameWithoutExtension(attachment.Name);
                                    string ext = Path.GetExtension(attachment.Name);
                                    filePath = Path.Combine(tempFolder, $"{nameWithoutExt}_{duplicateIndex}{ext}");
                                    duplicateIndex++;
                                }

                                // Save the attachment to the temporary folder
                                attachment.Save(filePath);
                                Console.WriteLine($"Saved attachment to: {filePath}");
                            }
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
