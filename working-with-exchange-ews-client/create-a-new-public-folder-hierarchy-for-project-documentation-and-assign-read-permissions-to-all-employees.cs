using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);

            using (client)
            {
                // Prepare permission for all employees (default user) with read access
                ExchangeFolderUserInfo defaultUser = ExchangeFolderUserInfo.DefaultUser;
                ExchangeFolderPermission permission = new ExchangeFolderPermission(defaultUser)
                {
                    ReadItems = ExchangeFolderPermissionReadAccess.FullDetails,
                    IsFolderVisible = true
                };

                ExchangeFolderPermissionCollection permissions = new ExchangeFolderPermissionCollection();
                permissions.Add(permission);

                // Create root public folder for project documentation
                ExchangeFolderInfo rootFolder = client.CreatePublicFolder("ProjectDocumentation", permissions);

                // Create subfolders under the root public folder
                client.CreateFolder(rootFolder.Uri, "Specs");
                client.CreateFolder(rootFolder.Uri, "Designs");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
