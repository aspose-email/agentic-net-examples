using Aspose.Email;
using System;
using System.Threading;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string sourceMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string targetMailboxUri = "https://exchange2.example.com/EWS/Exchange.asmx";
            string sourceUsername = "sourceUser";
            string sourcePassword = "sourcePass";
            string targetUsername = "targetUser";
            string targetPassword = "targetPass";
            string sourceFolderUri = null; // will be set to Inbox
            string targetFolderUri = null; // will be set to Inbox
            int syncIntervalSeconds = 300; // 5 minutes

            // Create source client
            using (IEWSClient sourceClient = EWSClient.GetEWSClient(sourceMailboxUri, new System.Net.NetworkCredential(sourceUsername, sourcePassword)))
            {
                // Create target client
                using (IEWSClient targetClient = EWSClient.GetEWSClient(targetMailboxUri, new System.Net.NetworkCredential(targetUsername, targetPassword)))
                {
                    // Resolve default Inbox folder URIs
                    sourceFolderUri = sourceClient.MailboxInfo.InboxUri;
                    targetFolderUri = targetClient.MailboxInfo.InboxUri;

                    while (true)
                    {
                        try
                        {
                            // Find all conversations in the source folder
                            ExchangeConversation[] conversations = sourceClient.FindConversations(sourceFolderUri);

                            foreach (ExchangeConversation conversation in conversations)
                            {
                                // Copy conversation items from source to target folder
                                targetClient.CopyConversationItems(conversation.ConversationId, targetFolderUri, sourceFolderUri);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Sync error: {ex.Message}");
                        }

                        // Wait before next sync cycle
                        Thread.Sleep(TimeSpan.FromSeconds(syncIntervalSeconds));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
