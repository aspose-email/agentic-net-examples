using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string localBackupFolder = "BackupAttachments";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure backup folder exists
            if (!Directory.Exists(localBackupFolder))
            {
                try
                {
                    Directory.CreateDirectory(localBackupFolder);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine("Failed to create backup folder: " + dirEx.Message);
                    return;
                }
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine("Failed to connect to Exchange server: " + clientEx.Message);
                return;
            }

            using (client)
            {
                // Get Inbox folder URI
                string inboxFolderUri = null;
                try
                {
                    // MailboxInfo provides the Inbox URI
                    inboxFolderUri = client.MailboxInfo.InboxUri;
                }
                catch (Exception inboxEx)
                {
                    Console.Error.WriteLine("Unable to retrieve Inbox folder URI: " + inboxEx.Message);
                    return;
                }

                // List messages with attachment information
                ExchangeMessageInfoCollection messages = null;
                try
                {
                    messages = client.ListMessages(inboxFolderUri, ExchangeListMessagesOptions.FetchAttachmentInformation);
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine("Failed to list messages: " + listEx.Message);
                    return;
                }

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Each message may have attachments; use UniqueUri to fetch the message if needed
                    if (messageInfo.HasAttachments)
                    {
                        // Fetch the full message to get attachment URIs
                        MailMessage fullMessage = null;
                        try
                        {
                            fullMessage = client.FetchMessage(messageInfo.UniqueUri);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueUri}: {fetchEx.Message}");
                            continue;
                        }

                        foreach (Attachment attachment in fullMessage.Attachments)
                        {
                            // Build local file path
                            string safeFileName = Path.GetFileName(attachment.Name);
                            string localPath = Path.Combine(localBackupFolder, safeFileName);

                            // Save attachment to local file
                            try
                            {
                                attachment.Save(localPath);
                                Console.WriteLine($"Saved attachment: {localPath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment {attachment.Name}: {saveEx.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
