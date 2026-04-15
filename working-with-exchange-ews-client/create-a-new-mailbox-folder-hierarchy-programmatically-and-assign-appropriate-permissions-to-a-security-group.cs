using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsFolderExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service URL and credentials (replace with actual values)
                string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Get the Inbox folder URI to serve as the parent for the new hierarchy
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo(client.MailboxInfo.InboxUri);
                    string parentFolderUri = inboxInfo.Uri;

                    // Define a security group (or user) to assign permissions to
                    ExchangeFolderUserInfo userInfo = new ExchangeFolderUserInfo();
                    userInfo.PrimarySmtpAddress = "group@example.com";
                    userInfo.DisplayName = "Example Security Group";

                    // Create a permission object for the user/group
                    ExchangeFolderPermission folderPermission = new ExchangeFolderPermission(userInfo);
                    folderPermission.PermissionLevel = ExchangeFolderPermissionLevel.Editor; // adjust as needed

                    // Add the permission to a collection
                    ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();
                    permissions.Add(folderPermission);

                    // Create the top‑level folder
                    ExchangeFolderInfo topFolder = client.CreateFolder(parentFolderUri, "TopFolder", permissions);
                    Console.WriteLine($"Created folder: {topFolder.DisplayName}");

                    // Create a subfolder under the newly created folder
                    ExchangeFolderInfo subFolder = client.CreateFolder(topFolder.Uri, "SubFolder", permissions);
                    Console.WriteLine($"Created subfolder: {subFolder.DisplayName}");

                    // Optionally, retrieve and display the permissions of the subfolder
                    ExchangePermissionCollection existingPermissions = client.GetFolderPermissions(subFolder.Uri);
                    foreach (ExchangeFolderPermission perm in existingPermissions)
                    {
                        Console.WriteLine($"User: {perm.UserInfo.PrimarySmtpAddress}, Level: {perm.PermissionLevel}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
