using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real server details when available.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the WebDAV client and ensure it is disposed properly.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve general mailbox information.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    Console.WriteLine("Mailbox Information:");
                    Console.WriteLine($"  Inbox URI: {mailboxInfo.InboxUri}");
                    Console.WriteLine($"  Sent Items URI: {mailboxInfo.SentItemsUri}");
                    Console.WriteLine($"  Drafts URI: {mailboxInfo.DraftsUri}");
                    Console.WriteLine($"  Deleted Items URI: {mailboxInfo.DeletedItemsUri}");
                    Console.WriteLine($"  Calendar URI: {mailboxInfo.CalendarUri}");
                    Console.WriteLine($"  Contacts URI: {mailboxInfo.ContactsUri}");
                    Console.WriteLine($"  Tasks URI: {mailboxInfo.TasksUri}");
                    Console.WriteLine();

                    // Helper to display folder details and message count.
                    void ShowFolderInfo(string folderName, string folderUri)
                    {
                        try
                        {
                            // Get folder properties.
                            var folderInfo = client.GetFolderInfo(folderUri);
                            Console.WriteLine($"{folderName} Folder:");
                            Console.WriteLine($"  URI: {folderInfo.Uri}");
                            Console.WriteLine($"  Display Name: {folderInfo.DisplayName}");
                            Console.WriteLine($"  Total Items: {folderInfo.TotalCount}");
                            Console.WriteLine($"  Unread Items: {folderInfo.UnreadCount}");

                            // List messages to obtain an accurate count.
                            ExchangeMessageInfoCollection messages = client.ListMessages(folderUri);
                            Console.WriteLine($"  Message Count (via ListMessages): {messages.Count}");
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error retrieving info for {folderName}: {ex.Message}");
                        }
                    }

                    // Display information for key folders.
                    ShowFolderInfo("Inbox", mailboxInfo.InboxUri);
                    ShowFolderInfo("Sent Items", mailboxInfo.SentItemsUri);
                    ShowFolderInfo("Drafts", mailboxInfo.DraftsUri);
                    ShowFolderInfo("Deleted Items", mailboxInfo.DeletedItemsUri);
                    ShowFolderInfo("Calendar", mailboxInfo.CalendarUri);
                    ShowFolderInfo("Contacts", mailboxInfo.ContactsUri);
                    ShowFolderInfo("Tasks", mailboxInfo.TasksUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Client operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
