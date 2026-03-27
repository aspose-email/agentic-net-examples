using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Target folder name to locate
                string targetFolderName = "TargetFolder";

                // Search for the folder in the mailbox hierarchy
                ExchangeFolderInfo foundFolder = FindFolder(client, targetFolderName);

                if (foundFolder != null)
                {
                    Console.WriteLine($"Found folder: {foundFolder.DisplayName}");
                    Console.WriteLine($"Folder URI: {foundFolder.Uri}");
                }
                else
                {
                    Console.WriteLine("Folder not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively searches for a folder by name starting from the root
    private static ExchangeFolderInfo FindFolder(IEWSClient client, string folderName)
    {
        // Get root folder information
        ExchangeFolderInfo rootFolder = client.GetFolderInfo("root");
        return FindFolderRecursive(client, rootFolder, folderName);
    }

    // Helper method that traverses subfolders depth‑first
    private static ExchangeFolderInfo FindFolderRecursive(IEWSClient client, ExchangeFolderInfo currentFolder, string folderName)
    {
        if (currentFolder.DisplayName.Equals(folderName, StringComparison.OrdinalIgnoreCase))
            return currentFolder;

        // Retrieve subfolders of the current folder
        ExchangeFolderInfoCollection subFolders = client.ListSubFolders(currentFolder.Uri);
        foreach (ExchangeFolderInfo subFolder in subFolders)
        {
            ExchangeFolderInfo result = FindFolderRecursive(client, subFolder, folderName);
            if (result != null)
                return result;
        }

        return null;
    }
}
