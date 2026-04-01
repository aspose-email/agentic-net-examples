using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping server call.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare a simple MAPI message to represent the new list item
                MapiMessage newListMessage = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "New List",
                    "This is a newly created list item.");

                // Create the item on the server; the method returns the item URI as a string
                string createdItemUri = client.CreateItem(newListMessage);

                Console.WriteLine("Item created with URI: " + createdItemUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
