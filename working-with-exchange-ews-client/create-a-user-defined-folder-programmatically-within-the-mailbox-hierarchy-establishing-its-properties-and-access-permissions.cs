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
            // EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Parent folder (Inbox) where the new folder will be created
                string parentFolderUri = client.MailboxInfo.InboxUri;
                string newFolderName = "MyCustomFolder";

                // Define the user who will have access to the new folder
                ExchangeFolderUserInfo userInfo = new ExchangeFolderUserInfo
                {
                    PrimarySmtpAddress = "user@example.com",
                    DisplayName = "User Name"
                };

                // Create a permission for the user
                ExchangeFolderPermission permission = new ExchangeFolderPermission(userInfo)
                {
                    PermissionLevel = ExchangeFolderPermissionLevel.Owner
                };

                // Add the permission to a collection
                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();
                permissions.Add(permission);

                // Create the folder with the specified permissions
                ExchangeFolderInfo folderInfo = client.CreateFolder(parentFolderUri, newFolderName, permissions);
                Console.WriteLine($"Folder created: {folderInfo.Uri}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
