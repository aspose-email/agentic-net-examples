using Aspose.Email.Storage.Pst;
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
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare permissions for the public folder
                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();

                // Define the group that will have read/write access
                ExchangeFolderUserInfo groupInfo = new ExchangeFolderUserInfo
                {
                    DisplayName = "All Employees",
                    PrimarySmtpAddress = "all@company.com"
                };

                ExchangeFolderPermission groupPermission = new ExchangeFolderPermission(groupInfo)
                {
                    PermissionLevel = ExchangeFolderPermissionLevel.PublishingEditor // read/write
                };

                permissions.Add(groupPermission);

                // Create the public folder with the specified permissions
                ExchangeFolderInfo newFolder = client.CreatePublicFolder("MyPublicFolder", permissions);

                // Publish (mail‑enable) the folder for organization‑wide access
                client.MailEnablePublicFolder(newFolder.Uri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
