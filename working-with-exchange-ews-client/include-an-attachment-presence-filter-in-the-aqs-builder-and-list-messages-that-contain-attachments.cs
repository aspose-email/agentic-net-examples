using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsAttachmentFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Build a query that filters messages having attachments
                    ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                    MailQuery attachmentQuery = queryBuilder.HasFlags(ExchangeMessageFlag.HasAttachment);

                    // Folder URI for the Inbox
                    string inboxFolderUri = client.MailboxInfo.InboxUri;

                    // List messages in the Inbox that match the attachment filter (recursive search)
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxFolderUri, attachmentQuery);

                    // Output subject and attachment presence for each message
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}, HasAttachments: {messageInfo.HasAttachments}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
