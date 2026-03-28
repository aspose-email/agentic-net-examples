using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the URI of the Inbox folder (parent folder)
                string inboxUri = client.MailboxInfo.InboxUri;

                // Prepare folder permissions
                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();

                ExchangeFolderUserInfo userInfo = new ExchangeFolderUserInfo();
                userInfo.PrimarySmtpAddress = "delegate@example.com";

                ExchangeFolderPermission permission = new ExchangeFolderPermission(userInfo);
                permission.PermissionLevel = ExchangeFolderPermissionLevel.Editor; // grant edit rights

                permissions.Add(permission);

                // Create a new folder under the Inbox with the specified permissions
                string folderName = "MyCustomFolder";
                ExchangeFolderInfo newFolder = client.CreateFolder(inboxUri, folderName, permissions);

                Console.WriteLine("Folder created: " + newFolder.Uri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
