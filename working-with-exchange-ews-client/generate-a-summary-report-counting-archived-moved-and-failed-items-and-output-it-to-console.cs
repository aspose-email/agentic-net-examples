using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with real server and credentials)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
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
                // Get Inbox folder information
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo(KnownFolders.Inbox.ToString());

                // Retrieve messages from Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxInfo.Uri);

                int archivedCount = 0;
                int movedCount = 0;
                int failedCount = 0;

                // Destination folder for moved items (Deleted Items used as example)
                ExchangeFolderInfo deletedItemsInfo = client.GetFolderInfo(KnownFolders.DeletedItems.ToString());

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // Unique identifier required for EWS operations
                    string itemUri = messageInfo.UniqueUri;

                    try
                    {
                        // Simple routing based on subject keywords
                        if (!string.IsNullOrEmpty(messageInfo.Subject) && messageInfo.Subject.Contains("Archive"))
                        {
                            // Archive the item (uses the overload that accepts a unique ID)
                            client.ArchiveItem(inboxInfo.Uri, itemUri);
                            archivedCount++;
                        }
                        else if (!string.IsNullOrEmpty(messageInfo.Subject) && messageInfo.Subject.Contains("Move"))
                        {
                            // Move the item to Deleted Items folder
                            client.MoveItem(itemUri, deletedItemsInfo.Uri);
                            movedCount++;
                        }
                        else
                        {
                            // Delete the item (move to Deleted Items)
                            client.DeleteItem(itemUri, new DeletionOptions(DeletionType.MoveToDeletedItems));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error and count as failed
                        Console.Error.WriteLine($"Operation failed for item {itemUri}: {ex.Message}");
                        failedCount++;
                    }
                }

                // Output summary report
                Console.WriteLine("Processing Summary:");
                Console.WriteLine($"Archived items: {archivedCount}");
                Console.WriteLine($"Moved items:    {movedCount}");
                Console.WriteLine($"Failed items:   {failedCount}");
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception handling
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
