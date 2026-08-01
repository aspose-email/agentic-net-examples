using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with real credentials and URL)
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Root folder URI in EWS
                string parentFolderUri = "msgfolderroot";

                // Name of the folder to locate
                string targetFolderName = "TargetFolder";

                // Find folders matching the specified name under the parent folder
                ExchangeFolderInfoCollection foundFolders = client.ListSubFolders(parentFolderUri, targetFolderName);

                if (foundFolders != null && foundFolders.Count > 0)
                {
                    var folder = foundFolders[0];
                    Console.WriteLine($"Found folder '{folder.DisplayName}' with URI: {folder.Uri}");
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
}
