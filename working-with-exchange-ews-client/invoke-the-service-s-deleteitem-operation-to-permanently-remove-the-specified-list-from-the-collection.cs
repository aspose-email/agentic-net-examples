using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailDeleteItemExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define service URL and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Example item URI to delete (replace with actual item URI)
                    string itemUri = "https://exchange.example.com/EWS/Exchange.asmx/Items/ABCDEF1234567890";

                    // Delete the item permanently
                    client.DeleteItem(itemUri, DeletionOptions.DeletePermanently);

                    Console.WriteLine("Item deleted permanently.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}