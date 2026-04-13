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
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to EWS: {ex.Message}");
                return;
            }

            using (client)
            {
                // Define permission for all employees (default user) – read only (Reviewer)
                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();
                ExchangeFolderPermission defaultPermission = new ExchangeFolderPermission(ExchangeFolderUserInfo.DefaultUser);
                defaultPermission.PermissionLevel = ExchangeFolderPermissionLevel.Reviewer;
                permissions.Add(defaultPermission);

                // Create root public folder for project documentation
                ExchangeFolderInfo rootFolderInfo = client.CreatePublicFolder("ProjectDocumentation", permissions);

                // Create subfolders under the root folder
                string parentUri = rootFolderInfo.Uri;

                client.CreatePublicFolder(parentUri, "Specs", permissions);
                client.CreatePublicFolder(parentUri, "Designs", permissions);
                client.CreatePublicFolder(parentUri, "Resources", permissions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
