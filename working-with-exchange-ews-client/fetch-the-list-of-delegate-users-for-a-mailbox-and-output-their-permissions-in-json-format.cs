using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string mailbox = "user@example.com";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password" || mailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    ExchangeDelegateUserCollection delegateCollection = client.ListDelegates(mailbox);
                    List<Dictionary<string, object>> delegateList = new List<Dictionary<string, object>>();

                    foreach (ExchangeDelegateUser delegateUser in delegateCollection)
                    {
                        ExchangeFolderUserInfo userInfo = delegateUser.UserInfo;
                        ExchangeDelegatePermissions permissions = delegateUser.FolderPermissions;

                        Dictionary<string, object> permissionDict = new Dictionary<string, object>
                        {
                            { "CalendarFolderPermissionLevel", permissions.CalendarFolderPermissionLevel.ToString() },
                            { "ContactsFolderPermissionLevel", permissions.ContactsFolderPermissionLevel.ToString() },
                            { "InboxFolderPermissionLevel", permissions.InboxFolderPermissionLevel.ToString() },
                            { "JournalFolderPermissionLevel", permissions.JournalFolderPermissionLevel.ToString() },
                            { "NotesFolderPermissionLevel", permissions.NotesFolderPermissionLevel.ToString() },
                            { "TasksFolderPermissionLevel", permissions.TasksFolderPermissionLevel.ToString() }
                        };

                        Dictionary<string, object> delegateDict = new Dictionary<string, object>
                        {
                            { "PrimarySmtpAddress", userInfo.PrimarySmtpAddress },
                            { "DisplayName", userInfo.DisplayName },
                            { "Permissions", permissionDict }
                        };

                        delegateList.Add(delegateDict);
                    }

                    string json = JsonSerializer.Serialize(delegateList, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving delegates: {ex.Message}");
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
