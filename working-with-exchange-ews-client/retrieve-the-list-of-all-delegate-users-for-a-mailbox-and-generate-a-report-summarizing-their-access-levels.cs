using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client inside a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // The mailbox for which to list delegates (can be the same as the authenticated user)
                string mailbox = "user@example.com";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || mailbox.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Retrieve the collection of delegate users
                ExchangeDelegateUserCollection delegateUsers = client.ListDelegates(mailbox);

                // Generate a simple report to the console
                Console.WriteLine($"Total delegates for mailbox '{mailbox}': {delegateUsers.Count}");
                Console.WriteLine();

                foreach (ExchangeDelegateUser delegateUser in delegateUsers)
                {
                    // Primary SMTP address of the delegate
                    string delegateEmail = delegateUser.UserInfo?.PrimarySmtpAddress ?? "N/A";

                    // Folder permissions granted to the delegate
                    ExchangeDelegatePermissions perms = delegateUser.FolderPermissions;

                    Console.WriteLine($"Delegate: {delegateEmail}");
                    Console.WriteLine($"  Calendar permission : {perms.CalendarFolderPermissionLevel}");
                    Console.WriteLine($"  Contacts permission : {perms.ContactsFolderPermissionLevel}");
                    Console.WriteLine($"  Inbox permission    : {perms.InboxFolderPermissionLevel}");
                    Console.WriteLine($"  Journal permission  : {perms.JournalFolderPermissionLevel}");
                    Console.WriteLine($"  Notes permission    : {perms.NotesFolderPermissionLevel}");
                    Console.WriteLine($"  Tasks permission    : {perms.TasksFolderPermissionLevel}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
