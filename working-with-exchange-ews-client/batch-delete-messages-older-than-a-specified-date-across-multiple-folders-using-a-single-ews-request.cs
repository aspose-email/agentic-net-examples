using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            try
            {
                // Define the cutoff date (messages older than this date will be deleted)
                DateTime cutoffDate = new DateTime(2023, 1, 1);

                // Build a MailQuery to filter messages by internal date
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.InternalDate.BeforeOrEqual(cutoffDate);
                MailQuery dateQuery = queryBuilder.GetQuery();

                // List of folder URIs to process (Inbox and Sent Items as examples)
                List<string> folderUris = new List<string>();
                folderUris.Add(client.MailboxInfo.InboxUri);
                folderUris.Add(client.MailboxInfo.SentItemsUri);

                // Collect URIs of messages that match the date criteria
                List<string> messageUrisToDelete = new List<string>();
                foreach (string folderUri in folderUris)
                {
                    // Retrieve messages in the folder that satisfy the date query
                    ExchangeMessageInfoCollection messages = client.ListMessages(folderUri, dateQuery);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Use UniqueUri for deletion operations
                        if (!string.IsNullOrEmpty(messageInfo.UniqueUri))
                        {
                            messageUrisToDelete.Add(messageInfo.UniqueUri);
                        }
                    }
                }

                // Perform batch deletion if there are messages to delete
                if (messageUrisToDelete.Count > 0)
                {
                    DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItems(messageUrisToDelete, deleteOptions);
                    Console.WriteLine("Deleted {0} messages older than {1}.", messageUrisToDelete.Count, cutoffDate.ToShortDateString());
                }
                else
                {
                    Console.WriteLine("No messages found older than the specified date.");
                }
            }
            finally
            {
                // Ensure the client is disposed
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
