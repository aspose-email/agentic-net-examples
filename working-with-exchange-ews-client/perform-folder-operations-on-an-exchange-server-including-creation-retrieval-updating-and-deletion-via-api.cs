using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution in CI
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder values to avoid unwanted network calls
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly
            using (client as IDisposable)
            {
                // Define folder names
                string parentFolderUri = client.MailboxInfo.InboxUri;
                string newFolderName = "SampleFolder";

                // 1. Create a new folder under Inbox
                ExchangeFolderInfo createdFolderInfo;
                try
                {
                    createdFolderInfo = client.CreateFolder(parentFolderUri, newFolderName);
                    Console.WriteLine($"Folder created: {createdFolderInfo.Uri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Folder creation failed: {ex.Message}");
                    return;
                }

                // 2. Retrieve information about the created folder
                ExchangeFolderInfo retrievedInfo;
                try
                {
                    retrievedInfo = client.GetFolderInfo(createdFolderInfo.Uri);
                    Console.WriteLine($"Retrieved folder display name: {retrievedInfo.DisplayName}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve folder info: {ex.Message}");
                }

                // 3. List subfolders of the parent folder (Inbox)
                try
                {
                    Console.WriteLine("Subfolders of Inbox:");
                    foreach (ExchangeFolderInfo subFolder in client.ListSubFolders(parentFolderUri))
                    {
                        Console.WriteLine($"- {subFolder.DisplayName} ({subFolder.Uri})");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list subfolders: {ex.Message}");
                }

                // 4. Delete the created folder
                try
                {
                    client.DeleteFolder(createdFolderInfo.Uri);
                    Console.WriteLine($"Folder deleted: {createdFolderInfo.Uri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Folder deletion failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
