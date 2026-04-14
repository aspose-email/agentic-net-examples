using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration – replace with actual values or obtain from environment
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Determine the folder for which permissions are required (root folder of the mailbox)
                    string folderUrl = client.MailboxInfo.RootUri;

                    // Retrieve folder permissions
                    ExchangePermissionCollection permissions = client.GetFolderPermissions(folderUrl);

                    // Prepare a list of serializable permission objects
                    List<object> permissionList = new List<object>();

                    foreach (ExchangeBasePermission basePerm in permissions)
                    {
                        // Most folder permissions are of type ExchangeFolderPermission
                        if (basePerm is ExchangeFolderPermission folderPerm)
                        {
                            permissionList.Add(new
                            {
                                DisplayName = folderPerm.UserInfo?.DisplayName,
                                PrimarySmtpAddress = folderPerm.UserInfo?.PrimarySmtpAddress,
                                PermissionLevel = folderPerm.PermissionLevel.ToString(),
                                CanCreateItems = folderPerm.CanCreateItems,
                                CanCreateSubFolders = folderPerm.CanCreateSubFolders,
                                DeleteItems = folderPerm.DeleteItems,
                                EditItems = folderPerm.EditItems,
                                IsFolderOwner = folderPerm.IsFolderOwner,
                                IsFolderContact = folderPerm.IsFolderContact,
                                IsFolderVisible = folderPerm.IsFolderVisible,
                                ReadItems = folderPerm.ReadItems
                            });
                        }
                    }

                    // Serialize the list to formatted JSON
                    string jsonOutput = JsonSerializer.Serialize(permissionList, new JsonSerializerOptions { WriteIndented = true });

                    // Output the JSON to the console
                    Console.WriteLine(jsonOutput);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving permissions: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
