using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
 // Required for ExchangeFolderInfo
 // For ExchangeFolderInfo type
using Aspose.Email.Clients.Exchange; // For ExchangeMessageInfoCollection

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create and use the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Get the URI of the Deleted Items folder
                    string deletedItemsFolderUri = client.MailboxInfo.DeletedItemsUri;

                    // List all messages in Deleted Items
                    ExchangeMessageInfoCollection messages = client.ListMessages(deletedItemsFolderUri);

                    // Determine the cutoff date (90 days ago)
                    DateTime cutoffDate = DateTime.UtcNow.AddDays(-90);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Use InternalDate for the message's received time
                        DateTime messageDate = messageInfo.InternalDate;

                        if (messageDate < cutoffDate)
                        {
                            try
                            {
                                // Delete the message using its unique URI
                                client.DeleteItem(messageInfo.UniqueUri, new DeletionOptions(DeletionType.MoveToDeletedItems));
                                Console.WriteLine($"Deleted message with URI: {messageInfo.UniqueUri}");
                            }
                            catch (Exception exDelete)
                            {
                                Console.Error.WriteLine($"Failed to delete message {messageInfo.UniqueUri}: {exDelete.Message}");
                            }
                        }
                    }
                }
                catch (Exception exClient)
                {
                    Console.Error.WriteLine($"Error during mailbox operations: {exClient.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
