using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual values)
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Keyword to search in conversation subject
                string keyword = "Invoice";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Collect all folder URIs starting from the Inbox
                List<string> allFolderUris = new List<string>();
                string startFolderUri = client.MailboxInfo.InboxUri;
                CollectFolderUris(client, startFolderUri, allFolderUris);

                // Search conversations in each folder
                foreach (string folderUri in allFolderUris)
                {
                    ExchangeConversation[] conversations = client.FindConversations(folderUri);
                    foreach (ExchangeConversation conversation in conversations)
                    {
                        string topic = conversation.ConversationTopic;
                        if (!string.IsNullOrEmpty(topic) &&
                            topic.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine($"Found conversation in folder '{folderUri}':");
                            Console.WriteLine($"  Topic: {topic}");
                            Console.WriteLine($"  ID: {conversation.ConversationId}");
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

    // Recursively collects folder URIs
    private static void CollectFolderUris(IEWSClient client, string folderUri, List<string> folderList)
    {
        if (client == null || string.IsNullOrEmpty(folderUri) || folderList == null)
            return;

        folderList.Add(folderUri);

        try
        {
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                CollectFolderUris(client, subFolder.Uri, folderList);
            }
        }
        catch (Exception ex)
        {
            // Log and continue with other folders
            Console.Error.WriteLine($"Failed to list subfolders of '{folderUri}': {ex.Message}");
        }
    }
}
