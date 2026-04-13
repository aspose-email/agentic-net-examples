using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // The URI of the root folder to delete (replace with actual folder URI)
                string rootFolderUri = "/Root/Inbox/FolderToDelete";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Collect all folder URIs recursively
                List<string> folderUris = new List<string>();
                CollectFolderUris(client, rootFolderUri, folderUris);

                // Convert to StringCollection required by DeleteFolders overload
                StringCollection urisToDelete = new StringCollection();
                foreach (string uri in folderUris)
                {
                    urisToDelete.Add(uri);
                }

                // Delete all folders permanently (deep delete)
                client.DeleteFolders(urisToDelete, true);
                Console.WriteLine("Folder hierarchy deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively collects the URIs of the specified folder and all its subfolders
    private static void CollectFolderUris(IEWSClient client, string folderUri, List<string> collection)
    {
        // Add the current folder URI
        collection.Add(folderUri);

        // Retrieve subfolders of the current folder
        ExchangeFolderInfoCollection subFolders = client.ListSubFolders(folderUri, string.Empty);
        if (subFolders != null)
        {
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                // Recurse into each subfolder
                CollectFolderUris(client, subFolder.Uri, collection);
            }
        }
    }
}
