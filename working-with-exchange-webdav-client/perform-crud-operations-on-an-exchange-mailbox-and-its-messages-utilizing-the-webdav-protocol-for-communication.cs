using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create the Exchange WebDAV client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // -------------------- Create a subfolder under Inbox --------------------
                string parentFolderUri = client.MailboxInfo.InboxUri;
                string newFolderName = "TestFolder";

                // CreateFolder(parentFolderUri, name) requires both parameters.
                client.CreateFolder(parentFolderUri, newFolderName);
                Console.WriteLine($"Folder '{newFolderName}' created under Inbox.");

                // -------------------- List messages in Inbox --------------------
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection inboxMessages =
                    client.ListMessages(client.MailboxInfo.InboxUri);
                Console.WriteLine($"Inbox contains {inboxMessages.Count} messages.");

                // -------------------- Create a new email message --------------------
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Message via WebDAV";
                message.Body = "This is a test email created by Aspose.Email WebDAV client.";

                // -------------------- Append the message to the newly created folder --------------------
                string newFolderUri = $"{parentFolderUri}/{newFolderName}";
                string createdMessageUri = client.AppendMessage(newFolderUri, message);
                Console.WriteLine($"Message appended to folder. URI: {createdMessageUri}");

                // -------------------- List messages in the new folder --------------------
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection folderMessages =
                    client.ListMessages(newFolderUri);
                Console.WriteLine($"Folder '{newFolderName}' now contains {folderMessages.Count} message(s).");

                // -------------------- Move the newly created message to Deleted Items --------------------
                // Retrieve the message info from the collection (assume the last one is the new message).
                Aspose.Email.Clients.Exchange.ExchangeMessageInfo msgInfo = null;
                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in folderMessages)
                {
                    msgInfo = info; // capture the last iterated item
                }

                if (msgInfo != null)
                {
                    string deletedItemsUri = client.MailboxInfo.DeletedItemsUri;
                    client.MoveMessage(msgInfo, deletedItemsUri);
                    Console.WriteLine("Message moved to Deleted Items folder.");
                }
                else
                {
                    Console.Error.WriteLine("Failed to locate the newly created message for moving.");
                }

                // -------------------- Delete the test folder --------------------
                client.DeleteFolder(newFolderUri);
                Console.WriteLine($"Folder '{newFolderName}' deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
