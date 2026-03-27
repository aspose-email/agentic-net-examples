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
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/exchange/user@example.com/";
            string username = "user@example.com";
            string password = "password";

            // Create and use the Exchange WebDAV client
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(mailboxUri, username, password))
            {
                // Get mailbox information
                Aspose.Email.Clients.Exchange.ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                Console.WriteLine("Mailbox Information:");
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                Console.WriteLine("Deleted Items URI: " + mailboxInfo.DeletedItemsUri);
                Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                Console.WriteLine("Contacts URI: " + mailboxInfo.ContactsUri);
                Console.WriteLine();

                // List top‑level folders (Inbox as example)
                string[] topFolders = new string[] { mailboxInfo.InboxUri, mailboxInfo.SentItemsUri, mailboxInfo.DraftsUri, mailboxInfo.DeletedItemsUri, mailboxInfo.CalendarUri, mailboxInfo.ContactsUri };
                foreach (string folderUri in topFolders)
                {
                    // Get folder metadata
                    Aspose.Email.Clients.Exchange.ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);

                    Console.WriteLine("Folder: " + folderInfo.DisplayName);
                    Console.WriteLine("URI: " + folderInfo.Uri);
                    Console.WriteLine("Total items: " + folderInfo.TotalCount);
                    Console.WriteLine("Unread items: " + folderInfo.UnreadCount);
                    Console.WriteLine("Size (bytes): " + folderInfo.Size);
                    Console.WriteLine();

                    // Get message count in the folder
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(folderInfo.Uri);
                    Console.WriteLine("Message count in '" + folderInfo.DisplayName + "': " + messages.Count);
                    Console.WriteLine(new string('-', 40));
                }

                // Optionally, enumerate subfolders of the Inbox
                Aspose.Email.Clients.Exchange.ExchangeFolderInfo inboxInfo = client.GetFolderInfo(mailboxInfo.InboxUri);
                Aspose.Email.Clients.Exchange.ExchangeFolderInfoCollection subFolders = client.ListSubFolders(inboxInfo.Uri);
                foreach (Aspose.Email.Clients.Exchange.ExchangeFolderInfo subFolder in subFolders)
                {
                    Console.WriteLine("Subfolder: " + subFolder.DisplayName);
                    Console.WriteLine("URI: " + subFolder.Uri);
                    Console.WriteLine("Total items: " + subFolder.TotalCount);
                    Console.WriteLine("Unread items: " + subFolder.UnreadCount);
                    Console.WriteLine("Size (bytes): " + subFolder.Size);
                    Console.WriteLine();

                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection subMessages = client.ListMessages(subFolder.Uri);
                    Console.WriteLine("Message count in subfolder '" + subFolder.DisplayName + "': " + subMessages.Count);
                    Console.WriteLine(new string('=', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}