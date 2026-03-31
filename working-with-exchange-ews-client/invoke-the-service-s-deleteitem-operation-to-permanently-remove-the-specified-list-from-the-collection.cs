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
            // Placeholder service URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder data.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    // URI of the item to delete (replace with a real item URI).
                    string itemUri = "https://exchange.example.com/EWS/ItemId=YOUR_ITEM_ID";

                    // Permanently delete the item.
                    client.DeleteItem(itemUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine("Item deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during deletion: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
