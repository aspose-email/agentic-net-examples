using Aspose.Email.Storage.Pst;
using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server details)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Source folder – Inbox
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // Create a test email message
                MailMessage testMessage = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Archive Test Subject",
                    "This is a test message to be archived.");

                // Append the message to the Inbox
                client.AppendMessage(inboxFolderUri, testMessage);

                // Retrieve the newly added message's unique URI
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(inboxFolderUri);
                var newlyAdded = inboxMessages.FirstOrDefault(m => m.Subject == "Archive Test Subject");
                if (newlyAdded == null)
                {
                    Console.Error.WriteLine("Failed to locate the appended message in Inbox.");
                    return;
                }

                string messageUniqueId = newlyAdded.UniqueUri;

                // Archive the message
                client.ArchiveItem(inboxFolderUri, messageUniqueId);

                // Attempt to get the Archive folder info (using the folder name "Archive")
                ExchangeFolderInfo archiveFolderInfo = null;
                try
                {
                    archiveFolderInfo = client.GetFolderInfo("Archive");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Unable to retrieve Archive folder info: " + ex.Message);
                    return;
                }

                // List messages in the Archive folder and verify the archived item exists
                ExchangeMessageInfoCollection archiveMessages = client.ListMessages(archiveFolderInfo.Uri);
                bool foundInArchive = archiveMessages.Any(m => m.Subject == "Archive Test Subject");

                Console.WriteLine(foundInArchive
                    ? "The message was successfully archived and found in the Archive folder."
                    : "The message was not found in the Archive folder.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
