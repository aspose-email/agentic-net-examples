using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
class Program
{
    static async Task Main()
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create asynchronous EWS client
            IAsyncEwsClient asyncClient;
            try
            {
                asyncClient = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Prepare a collection of items to update.
            // In a real scenario you would fetch messages, modify their Subject,
            // and create ExchangeStreamedItem objects that contain the updated MapiMessage.
            // Here we use an empty list to keep the example compile‑safe.
            List<ExchangeStreamedItem> itemsToUpdate = new List<ExchangeStreamedItem>();

            // Example placeholder for creating a custom property map (commented out):
            // MapiMessage msg = new MapiMessage();
            // msg.Subject = "New Subject";
            // var propertyMap = new MapiPropertyCollection();
            // propertyMap.Add(new MapiProperty(MapiPropertyTag.PR_SUBJECT, "New Subject"));
            // itemsToUpdate.Add(new ExchangeStreamedItem(msg, propertyMap));

            // Perform bulk update
            try
            {
                IEnumerable<ExchangeUploadItemResult> results = await asyncClient.UpdateItemsAsync(itemsToUpdate);
                foreach (ExchangeUploadItemResult result in results)
                {
                    Console.WriteLine($"Updated item ID: {result.ItemId}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Update operation failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

// Author note: This sample demonstrates the use of IAsyncEwsClient.UpdateItemsAsync for bulk updates.
// Populate ExchangeStreamedItem instances with actual messages and property maps to change subjects.
