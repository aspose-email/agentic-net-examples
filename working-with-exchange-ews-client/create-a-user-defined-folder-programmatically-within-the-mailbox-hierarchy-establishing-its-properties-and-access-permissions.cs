using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the URI of the parent folder (e.g., Inbox)
                string parentFolderUri = client.MailboxInfo.InboxUri;

                // Define the user who will have access to the new folder
                ExchangeFolderUserInfo userInfo = new ExchangeFolderUserInfo
                {
                    PrimarySmtpAddress = "user@example.com",
                    DisplayName = "Sample User"
                };

                // Create a permission for the user (owner level)
                ExchangeFolderPermission permission = new ExchangeFolderPermission(userInfo)
                {
                    PermissionLevel = ExchangeFolderPermissionLevel.Owner
                };

                // Add the permission to a collection
                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();
                permissions.Add(permission);

                // Create the new folder with the specified permissions
                ExchangeFolderInfo newFolder = client.CreateFolder(parentFolderUri, "MyCustomFolder", permissions);

                Console.WriteLine("Folder created successfully:");
                Console.WriteLine("Name: " + newFolder.DisplayName);
                Console.WriteLine("URI: " + newFolder.Uri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}