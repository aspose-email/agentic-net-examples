using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Source folder (Inbox) URI
                string sourceFolderUri = client.MailboxInfo.InboxUri;

                // Define the date threshold (e.g., messages older than 6 months)
                DateTime thresholdDateUtc = DateTime.UtcNow.AddMonths(-6);

                // Build a query to select messages older than the threshold
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.InternalDate.Before(thresholdDateUtc);
                MailQuery dateQuery = queryBuilder.GetQuery();

                // Retrieve messages that match the query
                ExchangeMessageInfoCollection oldMessages = client.ListMessages(sourceFolderUri, dateQuery);

                // Archive each message
                foreach (ExchangeMessageInfo messageInfo in oldMessages)
                {
                    // ArchiveItem overload that accepts the unique item identifier
                    client.ArchiveItem(sourceFolderUri, messageInfo.UniqueUri);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
