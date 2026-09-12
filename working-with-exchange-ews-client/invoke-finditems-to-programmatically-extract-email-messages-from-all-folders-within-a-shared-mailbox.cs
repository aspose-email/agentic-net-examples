using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS connection parameters (replace with real values)
            string ewsUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "shared_mailbox_user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
            {
                // Process common default folders
                ProcessFolder(client, client.GetMailboxInfo().InboxUri);
                ProcessFolder(client, client.GetMailboxInfo().DraftsUri);
                ProcessFolder(client, client.GetMailboxInfo().SentItemsUri);
                ProcessFolder(client, client.GetMailboxInfo().DeletedItemsUri);
                ProcessFolder(client, client.GetMailboxInfo().CalendarUri);
                ProcessFolder(client, client.GetMailboxInfo().ContactsUri);
                ProcessFolder(client, client.GetMailboxInfo().TasksUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively processes a folder: lists messages, extracts them, then processes subfolders
    private static void ProcessFolder(IEWSClient client, string folderUri)
    {
        try
        {
            // List all messages in the current folder (null query = all)
            ExchangeMessageInfoCollection messages = client.ListMessages(client.GetMailboxInfo().MailboxUri, folderUri, null);
            foreach (ExchangeMessageInfo msgInfo in messages)
            {
                try
                {
                    // Fetch the full MailMessage
                    MailMessage message = client.FetchMessage(msgInfo.UniqueUri);
                    // Example extraction: write subject to console
                    Console.WriteLine($"Folder: {folderUri} | Subject: {message.Subject}");
                }
                catch (Exception exMsg)
                {
                    Console.Error.WriteLine($"Failed to fetch message {msgInfo.UniqueUri}: {exMsg.Message}");
                }
            }

            // Get subfolders of the current folder
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);
            foreach (var subFolder in subFolders)
            {
                // Recursively process each subfolder using its FolderId
                ProcessFolder(client, subFolder.Uri);
            }
        }
        catch (Exception exFolder)
        {
            Console.Error.WriteLine($"Failed to process folder {folderUri}: {exFolder.Message}");
        }
    }
}
