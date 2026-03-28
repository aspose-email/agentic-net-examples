using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailWebDavSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the WebDAV client
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    Console.WriteLine("Mailbox URIs:");
                    Console.WriteLine($"Inbox: {mailboxInfo.InboxUri}");
                    Console.WriteLine($"Sent Items: {mailboxInfo.SentItemsUri}");
                    Console.WriteLine($"Drafts: {mailboxInfo.DraftsUri}");
                    Console.WriteLine($"Deleted Items: {mailboxInfo.DeletedItemsUri}");
                    Console.WriteLine($"Outbox: {mailboxInfo.OutboxUri}");
                    Console.WriteLine($"Calendar: {mailboxInfo.CalendarUri}");
                    Console.WriteLine($"Contacts: {mailboxInfo.ContactsUri}");
                    Console.WriteLine($"Tasks: {mailboxInfo.TasksUri}");
                    Console.WriteLine($"Notes: {mailboxInfo.NotesUri}");
                    Console.WriteLine($"Root: {mailboxInfo.RootUri}");

                    // Helper to display folder message count
                    void DisplayFolderInfo(string folderName, string folderUri)
                    {
                        try
                        {
                            // Optional: fetch folder details
                            ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);
                            // Count messages in the folder
                            var messages = client.ListMessages(folderUri);
                            int count = messages?.Count ?? 0;
                            Console.WriteLine($"{folderName} - Messages: {count}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to retrieve info for {folderName}: {ex.Message}");
                        }
                    }

                    // Retrieve counts for common folders
                    DisplayFolderInfo("Inbox", mailboxInfo.InboxUri);
                    DisplayFolderInfo("Sent Items", mailboxInfo.SentItemsUri);
                    DisplayFolderInfo("Drafts", mailboxInfo.DraftsUri);
                    DisplayFolderInfo("Deleted Items", mailboxInfo.DeletedItemsUri);
                    DisplayFolderInfo("Outbox", mailboxInfo.OutboxUri);
                    DisplayFolderInfo("Calendar", mailboxInfo.CalendarUri);
                    DisplayFolderInfo("Contacts", mailboxInfo.ContactsUri);
                    DisplayFolderInfo("Tasks", mailboxInfo.TasksUri);
                    DisplayFolderInfo("Notes", mailboxInfo.NotesUri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
