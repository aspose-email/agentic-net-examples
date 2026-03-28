using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExchangeFolderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server connection settings
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and connect the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // -----------------------------------------------------------------
                        // 1. Create a new folder under the Inbox
                        // -----------------------------------------------------------------
                        string parentFolderUri = client.MailboxInfo.InboxUri;
                        string newFolderName = "SampleFolder";

                        try
                        {
                            client.CreateFolder(parentFolderUri, newFolderName);
                            Console.WriteLine($"Folder '{newFolderName}' created under '{parentFolderUri}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating folder: {ex.Message}");
                        }

                        // -----------------------------------------------------------------
                        // 2. Retrieve information about the newly created folder
                        // -----------------------------------------------------------------
                        string newFolderUri = $"{parentFolderUri}/{newFolderName}";
                        try
                        {
                            ExchangeFolderInfo folderInfo = client.GetFolderInfo(newFolderUri);
                            Console.WriteLine($"Folder Info: Uri = {folderInfo.Uri}, DisplayName = {folderInfo.DisplayName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error retrieving folder info: {ex.Message}");
                        }

                        // -----------------------------------------------------------------
                        // 3. Update folder (example: set a custom property or permission)
                        //    For demonstration, we'll just output that an update could be placed here.
                        // -----------------------------------------------------------------
                        // Note: Specific update methods depend on the required operation.
                        // This placeholder shows where such logic would be inserted.

                        // -----------------------------------------------------------------
                        // 4. Delete the folder
                        // -----------------------------------------------------------------
                        try
                        {
                            client.DeleteFolder(newFolderUri);
                            Console.WriteLine($"Folder '{newFolderUri}' deleted.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
