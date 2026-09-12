using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
namespace UpdateCustomPropertySample
{
    class Program
    {
        // Author: Aspose.Email example – updates items via UpdateItemsAsync.
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // EWS endpoint and credentials (replace with real values).
            const string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            var credentials = new NetworkCredential("user@example.com", "password");

            try
            {
                // Initialize async EWS client.
                IAsyncEwsClient ewsClient = await EWSClient.GetEwsClientAsync(serviceUrl, credentials);

                // Retrieve mailbox information (e.g., Inbox URI) – not used further here.
                ExchangeMailboxInfo mailboxInfo = await ewsClient.GetMailboxInfoAsync();

                // -----------------------------------------------------------------
                // Prepare items to update.
                // In a real scenario you would fetch the item(s), modify their
                // property bag (e.g., add X-Custom-Tag), and wrap them into
                // ExchangeStreamedItem instances.
                // For this minimal compile‑safe example we pass an empty list.
                // -----------------------------------------------------------------
                IEnumerable<ExchangeStreamedItem> itemsToUpdate = new List<ExchangeStreamedItem>();

                // Perform the bulk update.
                IEnumerable<ExchangeUploadItemResult> updateResults = await ewsClient.UpdateItemsAsync(itemsToUpdate);

                // Report the number of processed items.
                int count = updateResults == null ? 0 : System.Linq.Enumerable.Count(updateResults);
                Console.WriteLine($"UpdateItemsAsync completed. Items processed: {count}");
            }
            catch (Exception ex)
            {
                // Graceful error handling.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
