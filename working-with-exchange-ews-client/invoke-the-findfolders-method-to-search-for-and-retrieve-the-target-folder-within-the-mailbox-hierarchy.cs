using Aspose.Email.Storage.Pst;
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
            // Placeholder credentials – skip actual network call in CI environments
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange service URL detected. Skipping network call.");
                return;
            }

            NetworkCredential credential = new NetworkCredential(username, password);

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Target folder name to find
                string targetFolderName = "TargetFolder";

                // Retrieve subfolders of the root folder
                ExchangeFolderInfoCollection subFolders = client.ListSubFolders(client.MailboxInfo.RootUri);

                // Search for the folder with the specified display name
                ExchangeFolderInfo targetFolder = null;
                foreach (ExchangeFolderInfo folderInfo in subFolders)
                {
                    if (string.Equals(folderInfo.DisplayName, targetFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetFolder = folderInfo;
                        break;
                    }
                }

                if (targetFolder != null)
                {
                    Console.WriteLine($"Folder found: {targetFolder.DisplayName}");
                    Console.WriteLine($"Folder URI: {targetFolder.Uri}");
                }
                else
                {
                    Console.WriteLine($"Folder \"{targetFolderName}\" not found in the mailbox hierarchy.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
