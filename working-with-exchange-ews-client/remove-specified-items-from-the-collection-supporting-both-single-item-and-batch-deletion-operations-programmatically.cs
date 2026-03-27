using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // ----- Single item deletion -----
                string singleItemUri = "item-uri-1";
                try
                {
                    client.DeleteItem(singleItemUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine("Deleted single item: " + singleItemUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error deleting single item: " + ex.Message);
                }

                // ----- Batch deletion -----
                List<string> batchItemUris = new List<string>
                {
                    "item-uri-2",
                    "item-uri-3",
                    "item-uri-4"
                };
                try
                {
                    // Cast to the async interface to use DeleteItemsAsync
                    IAsyncEwsClient asyncClient = (IAsyncEwsClient)client;
                    Task deleteTask = asyncClient.DeleteItemsAsync(batchItemUris, DeletionOptions.DeletePermanently);
                    deleteTask.GetAwaiter().GetResult();
                    Console.WriteLine("Deleted batch items successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error deleting batch items: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}