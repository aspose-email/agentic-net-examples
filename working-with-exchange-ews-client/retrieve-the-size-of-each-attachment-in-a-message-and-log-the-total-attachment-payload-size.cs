using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Server connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new System.Net.NetworkCredential(username, password)))
            {
                // Get the Inbox folder information
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo(client.MailboxInfo.InboxUri);

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxInfo.Uri);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Fetch the full mail message
                    MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueUri);

                    long totalAttachmentSize = 0;

                    // Calculate total size of all attachments
                    foreach (Attachment attachment in mailMessage.Attachments)
                    {
                        if (attachment.ContentStream != null)
                        {
                            totalAttachmentSize += attachment.ContentStream.Length;
                        }
                    }

                    Console.WriteLine($"Subject: {mailMessage.Subject}");
                    Console.WriteLine($"Total attachment payload size: {totalAttachmentSize} bytes");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
