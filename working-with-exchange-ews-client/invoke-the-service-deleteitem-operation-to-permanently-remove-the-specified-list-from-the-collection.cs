using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // URI of the item to be permanently deleted
                    string itemUri = "https://exchange.example.com/EWS/Item/12345";

                    // Delete the item permanently
                    client.DeleteItem(itemUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine("Item deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting item: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
