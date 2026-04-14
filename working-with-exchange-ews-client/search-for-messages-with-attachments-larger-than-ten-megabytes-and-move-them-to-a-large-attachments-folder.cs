using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client (WebDAV)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Ensure the target folder exists
                string targetFolderName = "Large Attachments";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string rootFolderUri = client.MailboxInfo.RootUri;
                string targetFolderUri;

                ExchangeFolderInfo existingFolderInfo;
                if (!client.FolderExists(rootFolderUri, targetFolderName, out existingFolderInfo))
                {
                    ExchangeFolderInfo createdFolder = client.CreateFolder(rootFolderUri, targetFolderName);
                    targetFolderUri = createdFolder.Uri;
                }
                else
                {
                    targetFolderUri = existingFolderInfo.Uri;
                }

                // List all messages in the Inbox
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo msgInfo in inboxMessages)
                {
                    // Fetch the full message to inspect attachments
                    MailMessage message = client.FetchMessage(msgInfo.UniqueUri);

                    long totalAttachmentSize = 0;
                    foreach (Attachment attachment in message.Attachments)
                    {
                        if (attachment.ContentStream != null)
                        {
                            totalAttachmentSize += attachment.ContentStream.Length;
                        }
                    }

                    // If total attachment size exceeds 10 MB, move the message
                    if (totalAttachmentSize > 10L * 1024 * 1024)
                    {
                        client.MoveItem(msgInfo.UniqueUri, targetFolderUri);
                        Console.WriteLine($"Moved message \"{msgInfo.Subject}\" to \"{targetFolderName}\".");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
