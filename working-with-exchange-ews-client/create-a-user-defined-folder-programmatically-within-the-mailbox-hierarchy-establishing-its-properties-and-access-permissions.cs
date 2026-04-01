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
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Define the parent folder (e.g., Inbox).
                    string parentFolderUri = client.MailboxInfo.InboxUri;

                    // Prepare folder permissions.
                    var permissions = new ExchangeFolderPermissionCollection();

                    var userInfo = new ExchangeFolderUserInfo
                    {
                        PrimarySmtpAddress = "user@example.com",
                        DisplayName = "User"
                    };

                    var folderPermission = new ExchangeFolderPermission(userInfo)
                    {
                        CanCreateItems = true,
                        CanCreateSubFolders = true,
                        IsFolderVisible = true,
                        IsFolderOwner = true
                    };

                    permissions.Add(folderPermission);

                    // Create the custom folder with permissions.
                    string newFolderName = "MyCustomFolder";
                    ExchangeFolderInfo newFolder = client.CreateFolder(parentFolderUri, newFolderName, permissions);

                    Console.WriteLine($"Folder '{newFolderName}' created with URI: {newFolder.Uri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during folder creation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
