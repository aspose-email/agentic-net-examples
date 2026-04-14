using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize EWS client with credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Source folder (Inbox)
                string inboxUri = client.MailboxInfo.InboxUri;

                // Build query for messages older than one year
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.InternalDate.Before(DateTime.Now.AddYears(-1));
                MailQuery query = queryBuilder.GetQuery();

                // Retrieve messages matching the query
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Archive the message to the archive mailbox
                        client.ArchiveItem(inboxUri, messageInfo.UniqueUri);
                        Console.WriteLine($"Archived message: {messageInfo.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to archive message '{messageInfo.Subject}': {ex.Message}");
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
