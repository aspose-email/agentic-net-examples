using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client (replace with actual server, username, password)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Ensure the archive folder exists under the Inbox
                string inboxUri = client.MailboxInfo.InboxUri;
                ExchangeFolderInfo archiveFolderInfo;
                bool archiveExists = client.FolderExists(inboxUri, "LargeAttachments", out archiveFolderInfo);
                if (!archiveExists)
                {
                    archiveFolderInfo = client.CreateFolder(inboxUri, "LargeAttachments");
                }
                string archiveFolderUri = archiveFolderInfo.Uri;

                // List all messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Fetch the full message to inspect attachments
                    MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                    bool hasLargeAttachment = false;
                    foreach (Attachment attachment in mailMessage.Attachments)
                    {
                        // Consider attachments larger than 5 MB as large
                        if (attachment.ContentStream != null && attachment.ContentStream.Length > 5 * 1024 * 1024)
                        {
                            hasLargeAttachment = true;
                            break;
                        }
                    }

                    // Move the message to the archive folder if a large attachment is found
                    if (hasLargeAttachment)
                    {
                        try
                        {
                            client.MoveItem(messageInfo.UniqueUri, archiveFolderUri);
                            Console.WriteLine($"Moved message '{messageInfo.Subject}' to archive folder.");
                        }
                        catch (Exception moveEx)
                        {
                            Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {moveEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
