using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the EWS service URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            var credentials = new System.Net.NetworkCredential("username", "password");

            // Create the EWS client inside a using block to ensure proper disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // List of folder URIs to process. Adjust as needed.
                string[] folderUris = { "Inbox", "SentItems", "Archive" };

                // Define the cutoff date; messages older than this will be deleted.
                DateTime cutoffDate = new DateTime(2023, 1, 1);

                // Build the mail query to find messages with InternalDate before the cutoff.
                MailQuery query = new MailQueryBuilder().InternalDate.Before(cutoffDate);

                // Collect the unique URIs of messages that match the criteria.
                List<string> messagesToDelete = new List<string>();

                foreach (string folderUri in folderUris)
                {
                    try
                    {
                        // Retrieve messages from the current folder that satisfy the query.
                        ExchangeMessageInfoCollection messages = client.ListMessages(folderUri, query);

                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            // UniqueUri is the identifier required for DeleteItems.
                            messagesToDelete.Add(messageInfo.UniqueUri);
                        }
                    }
                    catch (Exception exFolder)
                    {
                        Console.Error.WriteLine($"Error processing folder '{folderUri}': {exFolder.Message}");
                    }
                }

                // Perform batch deletion if any messages were found.
                if (messagesToDelete.Count > 0)
                {
                    try
                    {
                        DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                        client.DeleteItems(messagesToDelete, deleteOptions);
                        Console.WriteLine($"Deleted {messagesToDelete.Count} messages older than {cutoffDate:d}.");
                    }
                    catch (Exception exDelete)
                    {
                        Console.Error.WriteLine($"Error deleting messages: {exDelete.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No messages found matching the criteria.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
