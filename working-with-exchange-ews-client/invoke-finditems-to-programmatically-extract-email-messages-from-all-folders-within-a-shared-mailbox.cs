using System;
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
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Specify the shared mailbox address
                string sharedMailbox = "shared@example.com";

                // Retrieve mailbox information to obtain the root folder URI
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo(sharedMailbox);
                string rootFolderUri = mailboxInfo.RootUri;

                // Recursively process all folders and their messages
                ProcessFolder(client, sharedMailbox, rootFolderUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    static void ProcessFolder(IEWSClient client, string mailbox, string folderUri)
    {
        // List and fetch messages in the current folder
        try
        {
            ExchangeMessageInfoCollection messages = client.ListMessages(mailbox, folderUri, true);
            foreach (ExchangeMessageInfo messageInfo in messages)
            {
                using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                {
                    Console.WriteLine($"Folder: {folderUri}");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing messages in folder '{folderUri}': {ex.Message}");
        }

        // Recursively process subfolders
        try
        {
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                ProcessFolder(client, mailbox, subFolder.Uri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving subfolders of '{folderUri}': {ex.Message}");
        }
    }
}
