using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client.
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure proper disposal.
            using (client)
            {
                // Example: Delete a single item.
                string singleItemUri = "https://exchange.example.com/EWS/ItemId=AAABBBCCC";
                try
                {
                    DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItem(singleItemUri, deleteOptions);
                    Console.WriteLine("Single item deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting single item: {ex.Message}");
                }

                // Example: Delete multiple items in batch.
                List<string> itemUris = new List<string>
                {
                    "https://exchange.example.com/EWS/ItemId=DDD111",
                    "https://exchange.example.com/EWS/ItemId=EEE222",
                    "https://exchange.example.com/EWS/ItemId=FFF333"
                };
                try
                {
                    DeletionOptions batchDeleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItems(itemUris, batchDeleteOptions);
                    Console.WriteLine("Batch items deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting batch items: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
