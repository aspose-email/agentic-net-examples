using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the root folder URI of the shared mailbox
                string rootFolderUri = client.MailboxInfo.RootUri;

                // Collect all folder URIs recursively
                List<string> allFolderUris = new List<string>();
                CollectFolderUris(client, rootFolderUri, allFolderUris);

                // Iterate through each folder and extract messages
                foreach (string folderUri in allFolderUris)
                {
                    ExchangeMessageInfoCollection messages = client.ListMessages(folderUri);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Folder: {folderUri}, Subject: {messageInfo.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    // Recursively collects folder URIs starting from a given folder
    static void CollectFolderUris(IEWSClient client, string folderUri, List<string> folderUris)
    {
        folderUris.Add(folderUri);
        try
        {
            ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri);
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                CollectFolderUris(client, subFolder.Uri, folderUris);
            }
        }
        catch
        {
            // Ignore errors while retrieving subfolders
        }
    }
}
