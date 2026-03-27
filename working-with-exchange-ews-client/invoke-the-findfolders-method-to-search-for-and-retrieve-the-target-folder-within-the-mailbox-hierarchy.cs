using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define EWS service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // The name of the folder we want to find
                string targetFolderName = "TargetFolder";

                // Start searching from the root folder
                ExchangeFolderInfo rootFolder = client.GetFolderInfo(client.MailboxInfo.InboxUri);
                ExchangeFolderInfo foundFolder = FindFolderRecursive(client, rootFolder, targetFolderName);

                if (foundFolder != null)
                {
                    Console.WriteLine("Folder found:");
                    Console.WriteLine("Name: " + foundFolder.DisplayName);
                    Console.WriteLine("URI: " + foundFolder.Uri);
                }
                else
                {
                    Console.WriteLine("Folder '" + targetFolderName + "' not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Recursively searches subfolders for a folder with the specified name
    private static ExchangeFolderInfo FindFolderRecursive(IEWSClient client, ExchangeFolderInfo currentFolder, string targetName)
    {
        // Check the current folder
        if (string.Equals(currentFolder.DisplayName, targetName, StringComparison.OrdinalIgnoreCase))
        {
            return currentFolder;
        }

        // List immediate subfolders
        ExchangeFolderInfoCollection subFolders = client.ListSubFolders(currentFolder.Uri);
        foreach (ExchangeFolderInfo subFolder in subFolders)
        {
            // Recursively search each subfolder
            ExchangeFolderInfo result = FindFolderRecursive(client, subFolder, targetName);
            if (result != null)
            {
                return result;
            }
        }

        // Not found in this branch
        return null;
    }
}