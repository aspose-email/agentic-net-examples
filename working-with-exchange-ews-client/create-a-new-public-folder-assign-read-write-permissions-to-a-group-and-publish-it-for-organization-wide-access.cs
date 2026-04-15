using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsPublicFolderSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS client connection parameters
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
                {
                    // Define the user (group) that will have permissions on the new public folder
                    ExchangeFolderUserInfo groupInfo = new ExchangeFolderUserInfo
                    {
                        PrimarySmtpAddress = "group@example.com",
                        DisplayName = "Example Group"
                    };

                    // Create a permission object for the group
                    ExchangeFolderPermission groupPermission = new ExchangeFolderPermission(groupInfo)
                    {
                        // Grant read/write (Editor) permissions
                        PermissionLevel = ExchangeFolderPermissionLevel.Editor
                    };

                    // Assemble the permission collection
                    ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection
                    {
                        groupPermission
                    };

                    // Create the public folder in the root public folder
                    string newFolderName = "MyPublicFolder";

                    // Skip external calls when placeholder credentials are used
                    if (ewsUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    ExchangeFolderInfo newFolderInfo = client.CreatePublicFolder(newFolderName, permissions);

                    // Publish (mail‑enable) the folder for organization‑wide access
                    client.MailEnablePublicFolder(newFolderInfo.Uri);

                    Console.WriteLine($"Public folder '{newFolderName}' created and published successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
