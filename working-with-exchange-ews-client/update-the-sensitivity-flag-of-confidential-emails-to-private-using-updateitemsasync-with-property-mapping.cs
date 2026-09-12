using System.IO;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Exchange.WebService;
class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Connection parameters – replace with real values.
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create an asynchronous EWS client.
            IAsyncEwsClient ewsClient = await EWSClient.GetEwsClientAsync(
                mailboxUri,
                new NetworkCredential(username, password));

            // -----------------------------------------------------------------
            // Retrieve confidential email items and prepare them for update.
            // The actual retrieval (e.g., using FindItemsAsync with a filter on
            // Sensitivity = Confidential) and property mapping are omitted for
            // brevity. Below we create an empty list that would be populated with
            // ExchangeStreamedItem instances whose Sensitivity property is set to
            // Private.
            // -----------------------------------------------------------------
            List<ExchangeStreamedItem> itemsToUpdate = new List<ExchangeStreamedItem>();

            // Example of how an item could be added (commented out because it
            // requires a valid message stream and property mapping):
            // using (var messageStream = File.OpenRead("confidential.eml"))
            // {
            //     var propertyMap = new Dictionary<string, object>
            //     {
            //         { "Sensitivity", 2 } // 2 corresponds to Private
            //     };
            //     var streamedItem = new ExchangeStreamedItem(messageStream, propertyMap);
            //     itemsToUpdate.Add(streamedItem);
            // }

            // Update the items asynchronously.
            IEnumerable<ExchangeUploadItemResult> results = await ewsClient.UpdateItemsAsync(
                itemsToUpdate,
                CancellationToken.None);

            Console.WriteLine("Update operation completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
