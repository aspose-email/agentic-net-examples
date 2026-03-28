using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize credentials (replace with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client (variable name must be 'client')
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Create a MAPI message to be stored as a list item
                using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is a test message."))
                {
                    // Create the item on the server (default folder)
                    string itemId = client.CreateItem(message);
                    Console.WriteLine("Item created with ID: " + itemId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
